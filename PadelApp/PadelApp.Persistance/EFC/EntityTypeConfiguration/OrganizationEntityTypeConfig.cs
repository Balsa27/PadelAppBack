using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;
using PadelApp.Persistance.Constants;

namespace PadelApp.Persistance.EFC.EntityTypeConfiguration;

public class OrganizationEntityTypeConfig : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable(TableNames.Organizations);

        // Inherit properties from User
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedNever();
        builder.Property(o => o.Username).IsRequired().HasMaxLength(100);
        builder.Property(o => o.Email).IsRequired().HasMaxLength(100);
        builder.Property(o => o.Password).IsRequired();
        builder.Property(o => o.Role).HasConversion<string>();
        builder.Property(o => o.GoogleId);
        builder.Property(o => o.AppleId);
        
        builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
        builder.Property(o => o.Description).HasMaxLength(500);
        builder.Property(o => o.Status).HasConversion<string>();
        
        builder.OwnsOne(o => o.Address, a =>
        {
            a.ToTable(TableNames.OrganizationAddress); 
            a.WithOwner().HasForeignKey("OrganizationId");
        });
        
        builder.OwnsOne(o => o.ContactInfo, c =>
        {
            c.ToTable(TableNames.OrganizationContactInfo);
            c.WithOwner().HasForeignKey("OrganizationId");
        });


        builder.HasMany(o => o.Courts)
            .WithOne()
            .HasForeignKey(oc => oc.CourtId);
    }
}