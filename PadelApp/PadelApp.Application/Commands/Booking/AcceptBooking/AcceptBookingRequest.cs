namespace PadelApp.Application.Commands.Booking.AcceptBooking;

public record AcceptBookingRequest(Guid BookingId, Guid CourtId);
