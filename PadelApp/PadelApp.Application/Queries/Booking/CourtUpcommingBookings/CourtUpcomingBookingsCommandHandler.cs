using MediatR;
using PadelApp.Application.Abstractions.Repositories;

namespace PadelApp.Application.Queries.Booking.CourtUpcommingBookings;

public class CourtUpcomingBookingsCommandHandler : IRequestHandler<CourtUpcomingBookingsCommand, List<CourtUpcomingBookingsResponse>?>
{
    private readonly IBookingRepository _bookingRepository;

    public CourtUpcomingBookingsCommandHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<List<CourtUpcomingBookingsResponse>?> Handle(CourtUpcomingBookingsCommand request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetCourtUpcomingBookingsAsync(request.CourtId);
        
        if (bookings is null)
            return null;

        return bookings.Select(
            b => new CourtUpcomingBookingsResponse(
                b.CourtId,
                b.BookerId,
                b.StartTime,
                b.EndTime,
                b.Status))
            .ToList();
    }
}