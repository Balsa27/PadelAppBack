using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Entities;

namespace PadelApp.Domain.Events.DomainEvents.DomainEventConverter;

public class OrganizationCourtEntityTypeConfiguration : IEntityTypeConfiguration<OrganizationCourt>
{
    public void Configure(EntityTypeBuilder<OrganizationCourt> builder)
    {
        builder.HasKey(oc => new { oc.OrganizationId, oc.CourtId });
        builder.HasOne<Court>().WithMany().HasForeignKey(oc => oc.CourtId);
        builder.HasOne<Organization>().WithMany().HasForeignKey(oc => oc.OrganizationId);
    }
}