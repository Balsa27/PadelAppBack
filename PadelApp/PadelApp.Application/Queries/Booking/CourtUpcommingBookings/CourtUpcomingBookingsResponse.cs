using PadelApp.Domain.Enums;

namespace PadelApp.Application.Queries.Booking.CourtUpcommingBookings;

public record CourtUpcomingBookingsResponse(Guid CourtId,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime,
    BookingStatus Status);