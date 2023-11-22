using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.ValueObjects;
using PadelApp.Persistance.Constants;

namespace PadelApp.Persistance.EFC.EntityTypeConfiguration;

public class CourtEntityTypeConfiguration : IEntityTypeConfiguration<Court>
{
    public void Configure(EntityTypeBuilder<Court> builder)
    {
        builder.ToTable(TableNames.Courts);
        builder.HasKey(c => c.Id);
        builder.Property(c => c.OrganizationId).IsRequired();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.ProfileImage);
        builder.Property(c => c.Status).HasConversion<string>(); 
        builder.Property(c => c.WorkingEndTime).IsRequired().HasColumnType("time(6)");
        builder.Property(c => c.WorkingStartTime).IsRequired().HasColumnType("time(6)");
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.HasMany(c => c.Prices)
            .WithOne() 
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.OwnsOne(c => c.Address, c =>
        {
            c.WithOwner().HasForeignKey("CourtId");
            c.ToTable(TableNames.CourtAddress);
        });
        
        builder.HasMany(c => c.Bookings)
            .WithOne() 
            .HasForeignKey("CourtId") 
            .OnDelete(DeleteBehavior.Cascade); 
        
        builder.Property(c => c.CourtImages).HasConversion(
            v => string.Join(";", v),
            v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}