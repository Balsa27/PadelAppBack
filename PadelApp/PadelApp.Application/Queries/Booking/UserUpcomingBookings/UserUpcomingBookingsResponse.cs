namespace PadelApp.Application.Queries.Booking.UserUpcomingBookings;

public record UserUpcomingBookingsResponse(
    Guid CourtId,
    string CourtName,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime);
    
