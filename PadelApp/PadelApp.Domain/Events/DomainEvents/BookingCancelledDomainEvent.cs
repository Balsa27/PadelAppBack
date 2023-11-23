using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Events.DomainEvents;

public record BookingCancelledDomainEvent(Guid BookingId, List<Guid> WaitingList) : IDomainEvent;