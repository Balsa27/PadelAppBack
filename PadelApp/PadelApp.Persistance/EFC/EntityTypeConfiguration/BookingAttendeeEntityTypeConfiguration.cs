using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;
using PadelApp.Persistance.Constants;
using PadelApp.Persistance.Dbos;

namespace PadelApp.Persistance.EFC.EntityTypeConfiguration;

public class BookingAttendeeEntityTypeConfiguration : IEntityTypeConfiguration<BookingAttendee>
{
    public void Configure(EntityTypeBuilder<BookingAttendee> builder)
    {
        builder.ToTable(TableNames.BookingAttendees); // Define the table name
      
        builder.HasKey(ba => new { ba.BookingId, ba.PlayerId });
    }
}