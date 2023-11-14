using MediatR;
using PadelApp.Application.Abstractions.Repositories;

namespace PadelApp.Application.Queries.Booking.UserPendingBookings;

public class UserPendingBookingsRequestHandler : IRequestHandler<UserPendingBookingsCommand, List<UserPendingBookingsResponse>?>
{
    private readonly IBookingRepository _bookingRepository;

    public UserPendingBookingsRequestHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<List<UserPendingBookingsResponse>?> Handle(UserPendingBookingsCommand request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetUserPendingBookingAsync(request.UserId);
        
        if (bookings is null)
            return null;
        
        return bookings.Select(
                b => new UserPendingBookingsResponse(
                    b.CourtId,
                    b.CourtName,
                    b.BookerId,
                    b.StartTime,
                    b.EndTime))
            .ToList();
    }
}