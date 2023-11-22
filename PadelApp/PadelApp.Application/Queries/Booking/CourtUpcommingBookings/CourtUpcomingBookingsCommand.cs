using MediatR;

namespace PadelApp.Application.Queries.Booking.CourtUpcommingBookings;

public record CourtUpcomingBookingsCommand(Guid CourtId) : IRequest<List<CourtUpcomingBookingsResponse>?>;
