using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;
using PadelApp.Persistance.Constants;

namespace PadelApp.Persistance.EFC.EntityTypeConfiguration;

public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable(TableNames.Players);

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
            .HasConversion<int>() 
            .IsRequired();
        
        builder.Property(p => p.GoogleId)
            .HasMaxLength(100)
            .IsRequired(false);  // Make it optional if not all users will have Google IDs

        builder.Property(p => p.AppleId)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(p => p.IsActive)
            .HasMaxLength(100);
    }
}