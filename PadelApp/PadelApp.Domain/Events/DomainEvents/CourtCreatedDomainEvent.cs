using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Events.DomainEvents;

public record CourtCreatedDomainEvent(Guid CourtId, Guid OrganizationId) : IDomainEvent;
