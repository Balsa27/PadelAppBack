using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelApp.Domain.Entities;
using PadelApp.Persistance.Constants;

namespace PadelApp.Persistance.EFC.EntityTypeConfiguration;

public class OrganizationEntityTypeConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable(TableNames.Organizations);

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedNever(); 
        
        builder.Property(p => p.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Password)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(p => p.Role)
            .HasConversion<int>() //brze vjrv
            .IsRequired();
    }
}