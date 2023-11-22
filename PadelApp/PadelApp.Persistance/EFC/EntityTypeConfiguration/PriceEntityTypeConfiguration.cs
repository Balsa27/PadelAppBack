using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Persistance.EFC.EntityTypeConfiguration;

public class PriceEntityTypeConfiguration : IEntityTypeConfiguration<Price>
{
    public void Configure(EntityTypeBuilder<Price> builder)
    {
        builder.ToTable("CourtPrices"); // Or any other table name

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.Amount).IsRequired();
        builder.Property(p => p.Duration).IsRequired();

        builder.Property(p => p.Days)
            .HasConversion(
                v => string.Join(",", v.Select(d => (int)d)),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .Cast<DayOfWeek>()
                    .ToList())
            .HasColumnName("DaysOfWeek");

        builder.Property<Guid>("CourtId");
        builder.HasOne<Court>().WithMany(c => c.Prices).HasForeignKey("CourtId");
    }
}