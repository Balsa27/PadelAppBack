using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;
using PadelApp.Persistance.Constants;
using PadelApp.Persistance.Dbos;

namespace PadelApp.Persistance.EFC.EntityTypeConfiguration;

public class BookingEntityTypeConfig : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable(TableNames.Bookings);

        // Primary Key
        builder.HasKey(b => b.Id);

        // Properties
        builder.Property(b => b.CourtId).IsRequired();
        builder.Property(b => b.CourtName).IsRequired().HasMaxLength(100);

        // Booker relationship
        builder.HasOne<Player>()
            .WithMany()
            .HasForeignKey(b => b.BookerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Status
        builder.Property(b => b.Status).HasConversion<string>();

        // Time properties
        builder.Property(b => b.StartTime).IsRequired();
        builder.Property(b => b.EndTime).IsRequired();

        // WaitingList
        builder.OwnsOne(b => b.WaitingList, wl =>
        {
            wl.WithOwner().HasForeignKey("BookingId");
            wl.ToTable(TableNames.BookingWaitingList);
        });

        builder.HasMany(b => b.Attendees)
            .WithOne() // If there's no navigation property back to Booking in BookingAttendee
            .HasForeignKey(ba => ba.BookingId);
    }
}
