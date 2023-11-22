using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Abstractions.Repositories;

namespace PadelApp.Application.Queries.Booking.UserPendingBookings;

public class UserPendingBookingsRequestHandler : IRequestHandler<UserPendingBookingsCommand, List<UserPendingBookingsResponse>?>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserContextService _userContextService;

    public UserPendingBookingsRequestHandler(IBookingRepository bookingRepository, IUserContextService userContextService)
    {
        _bookingRepository = bookingRepository;
        _userContextService = userContextService;
    }

    public async Task<List<UserPendingBookingsResponse>?> Handle(UserPendingBookingsCommand request, CancellationToken cancellationToken)
    {
        var id = _userContextService.GetCurrentUserId();
        
        var bookings = await _bookingRepository.GetUserPendingBookingAsync(id);
        
        if (bookings is null)
            return null;
        
        return bookings.Select(
                b => new UserPendingBookingsResponse(
                    b.CourtId,
                    b.BookerId,
                    b.StartTime,
                    b.EndTime,
                    b.Status))
            .ToList();
    }
}