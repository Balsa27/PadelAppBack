using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Commands.Booking.AcceptBooking;

public record AcceptBookingCommand(Guid BookingId, Guid CourtId) : IRequest<AcceptBookingResponse>;
