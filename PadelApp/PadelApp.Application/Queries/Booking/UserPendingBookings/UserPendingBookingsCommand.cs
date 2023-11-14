using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Queries.Booking.UserPendingBookings;

public record UserPendingBookingsCommand() : IRequest<List<UserPendingBookingsResponse>?>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
    
