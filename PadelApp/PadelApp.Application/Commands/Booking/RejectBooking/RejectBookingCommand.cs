using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Commands.Booking.AcceptBooking;

namespace PadelApp.Application.Commands.Booking.RejectBooking;

public record RejectBookingCommand(Guid BookingId, Guid BookerId, Guid CourtId) 
    : IRequest<RejectBookingResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
