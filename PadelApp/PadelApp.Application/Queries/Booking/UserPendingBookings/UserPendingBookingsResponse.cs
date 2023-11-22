using PadelApp.Domain.Enums;

namespace PadelApp.Application.Queries.Booking.UserPendingBookings;

public record UserPendingBookingsResponse(
    Guid CourtId,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime,
    BookingStatus Status);