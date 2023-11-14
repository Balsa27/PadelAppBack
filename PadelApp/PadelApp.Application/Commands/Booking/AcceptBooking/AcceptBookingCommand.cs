using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Commands.Booking.AcceptBooking;

public record AcceptBookingCommand(Guid BookingId, Guid BookerId, Guid CourtId) : IRequest<AcceptBookingResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
