using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Queries.Booking.UserUpcomingBookings;

public record UserUpcomingBookingsCommand(Guid UserId) : IRequest<List<UserUpcomingBookingsResponse>?>;
