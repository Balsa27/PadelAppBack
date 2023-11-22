using PadelApp.Domain.Enums;

namespace PadelApp.Application.Queries.Booking.AllPendingBookings;

public record CourtPendingBookingsResponse(Guid CourtId,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime,
    BookingStatus Status);