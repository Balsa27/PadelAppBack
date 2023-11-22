using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;

namespace PadelApp.Application.Queries.Booking.UserUpcomingBookings;

public class UserUpcomingBookingsRequestHandler : IRequestHandler<UserUpcomingBookingsCommand, List<UserUpcomingBookingsResponse>?>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserContextService _userContextService;

    public UserUpcomingBookingsRequestHandler(IBookingRepository bookingRepository, IUserContextService userContextService)
    {
        _bookingRepository = bookingRepository;
        _userContextService = userContextService;
    }

    public async Task<List<UserUpcomingBookingsResponse>?> Handle(UserUpcomingBookingsCommand request, CancellationToken cancellationToken)
    {
        var id = _userContextService.GetCurrentUserId();
        var bookings = await _bookingRepository.GetUserUpcomingBookingAsync(id);

        if (bookings is null)
            return null;
        
        return bookings.Select(
            b => new UserUpcomingBookingsResponse(
                b.CourtId,
                b.BookerId,
                b.StartTime,
                b.EndTime,
                b.Status))
            .ToList();
    }
}