namespace PadelApp.Application.Queries.Booking.AllPendingBookings;

public record CourtPendingBookingsRequest(Guid CourtId, Guid BookerId);