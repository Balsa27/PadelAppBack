using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Events.DomainEvents.DomainEventConverter;

public record BookingRejectedDomainEvent(Guid BookingId, Guid BookerId) : IDomainEvent;
