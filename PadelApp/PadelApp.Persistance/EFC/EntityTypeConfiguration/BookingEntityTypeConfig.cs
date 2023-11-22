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
        builder.Property(p => p.Id).ValueGeneratedNever();

        // Properties
        builder.Property(b => b.CourtId).IsRequired();

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

   
        // builder.HasMany(b => b.Attendees)
        //     .WithOne() // If there's no navigation property back to Booking in BookingAttendee
        //     .HasForeignKey(ba => ba.BookingId);
    }
}
