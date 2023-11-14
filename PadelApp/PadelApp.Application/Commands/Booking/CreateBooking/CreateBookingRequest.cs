namespace PadelApp.Application.Commands.Player.CreateBooking;

public record CreateBookingRequest(Guid CourtId, string CourtName, Guid BookerId, DateTime StartTime, DateTime EndTime);