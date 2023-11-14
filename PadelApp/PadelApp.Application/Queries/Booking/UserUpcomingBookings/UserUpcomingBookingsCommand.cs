using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Queries.Booking.UserUpcomingBookings;

public record UserUpcomingBookingsCommand() : IRequest<List<UserUpcomingBookingsResponse>?>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
