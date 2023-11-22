using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Queries.Booking.UserPendingBookings;

public record UserPendingBookingsCommand() : IRequest<List<UserPendingBookingsResponse>?>;    
