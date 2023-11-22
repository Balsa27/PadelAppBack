using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Events.DomainEvents;

public record RemoveCourtFromOrganizationDomainEvent(Guid CourtId, Guid OrganizationId ) : IDomainEvent;
