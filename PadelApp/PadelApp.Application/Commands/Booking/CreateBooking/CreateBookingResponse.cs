using PadelApp.Domain.Enums;

namespace PadelApp.Application.Commands.Player.CreateBooking;

public record CreateBookingResponse(
    Guid BookingId,
    Guid CourtId,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime,
    BookingStatus Status);
