namespace PadelApp.Application.Commands.Player.CreateBooking;

public record CreateBookingRequest(Guid CourtId, Guid BookerId, DateTime StartTime, DateTime EndTime);