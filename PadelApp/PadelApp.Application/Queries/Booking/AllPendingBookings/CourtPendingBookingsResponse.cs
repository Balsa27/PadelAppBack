namespace PadelApp.Application.Queries.Booking.AllPendingBookings;

public record CourtPendingBookingsResponse(Guid CourtId,
    string CourtName,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime);