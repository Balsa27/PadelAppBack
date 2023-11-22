using PadelApp.Domain.Enums;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.DomainEvents;

public record CourtStatusChangeDomainEvent(
    Guid CourtId,
    DateTime LastOnGoingBookingEndTime,
    CourtStatus Status) : IDomainEvent;
