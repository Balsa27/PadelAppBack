using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;

namespace PadelApp.Application.Queries.Booking.UserUpcomingBookings;

public class UserUpcomingBookingsRequestHandler : IRequestHandler<UserUpcomingBookingsCommand, List<UserUpcomingBookingsResponse>?>
{
    private readonly IBookingRepository _bookingRepository;

    public UserUpcomingBookingsRequestHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<List<UserUpcomingBookingsResponse>?> Handle(UserUpcomingBookingsCommand request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetUserUpcomingBookingAsync(request.UserId);

        if (bookings is null)
            return null;
        
        return bookings.Select(
            b => new UserUpcomingBookingsResponse(
                b.CourtId,
                b.CourtName,
                b.BookerId,
                b.StartTime,
                b.EndTime))
            .ToList();
    }
}