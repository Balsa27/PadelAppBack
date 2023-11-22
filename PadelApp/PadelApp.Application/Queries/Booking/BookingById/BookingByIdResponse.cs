using PadelApp.Domain.Enums;

namespace PadelApp.Application.Queries.Booking.BookingById;

public record BookingByIdResponse(
    Guid BookingId,
    Guid CourtId,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime,
    BookingStatus Status);