using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Queries.Booking.AllPendingBookings;

public record CourtPendingBookingsCommand(Guid CourtId) : IRequest<List<CourtPendingBookingsResponse>?>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
