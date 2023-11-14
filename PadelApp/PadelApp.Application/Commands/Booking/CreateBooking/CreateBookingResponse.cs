namespace PadelApp.Application.Commands.Player.CreateBooking;

public record CreateBookingResponse(Guid BookingId, Guid CourtId, string CourtName, Guid BookerId, DateTime StartTime, DateTime EndTime);
