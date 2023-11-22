using MediatR;
using PadelApp.Application.Abstractions.Repositories;

namespace PadelApp.Application.Queries.Booking.AllPendingBookings;

public class CourtPendingBookingsRequestHandler : IRequestHandler<CourtPendingBookingsCommand, List<CourtPendingBookingsResponse>?>
{
    private readonly IBookingRepository _bookingRepository;

    public CourtPendingBookingsRequestHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<List<CourtPendingBookingsResponse>?> Handle(CourtPendingBookingsCommand request, CancellationToken cancellationToken)
    {
        var courtBookings = await _bookingRepository.GetCourtPendingBookingsAsync(request.CourtId);
        
        if (courtBookings is null)
            return null;

        return courtBookings.Select(
            b => new CourtPendingBookingsResponse(
                b.CourtId,
                b.BookerId,
                b.StartTime,
                b.EndTime,
                b.Status))
            .ToList();
    }
}