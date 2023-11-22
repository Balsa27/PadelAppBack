using PadelApp.Domain.Enums;

namespace PadelApp.Application.Queries.Booking.UserUpcomingBookings;

public record UserUpcomingBookingsResponse(
    Guid CourtId,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime,
    BookingStatus Status);
    
