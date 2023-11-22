using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Events.DomainEvents.DomainEventConverter;

public record BookingAcceptedDomainEvent(Guid BookingId, Guid BookerId) : IDomainEvent;
