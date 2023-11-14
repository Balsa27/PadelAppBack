namespace PadelApp.Application.Queries.Booking.UserPendingBookings;

public record UserPendingBookingsResponse(
    Guid CourtId,
    string CourtName,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime);