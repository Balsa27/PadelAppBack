using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PadelApp.Domain.Entities;

namespace PadelApp.Domain.Events.DomainEvents.DomainEventConverter;

public class OrganizationCourtEntityTypeConfiguration : IEntityTypeConfiguration<OrganizationCourt>
{
    public void Configure(EntityTypeBuilder<OrganizationCourt> builder)
    {
        builder.HasKey(oc => new { oc.OrganizationId, oc.CourtId });
    }
}