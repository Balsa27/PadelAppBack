using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Events.DomainEvents;

public record BookingCreatedDomainEvent(Guid OrganizationId) : IDomainEvent;
