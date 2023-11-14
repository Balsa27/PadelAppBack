using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelApp.Domain.Aggregates;
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
        
        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.OwnsOne(c => c.Address, a =>
        {
            a.WithOwner();
        });
        
        builder.OwnsMany(c => c.Prices, p =>
        {
            p.WithOwner().HasForeignKey("CourtId");
            p.ToTable(TableNames.CourtPrices);
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