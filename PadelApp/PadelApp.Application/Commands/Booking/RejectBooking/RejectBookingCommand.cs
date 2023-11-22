using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Commands.Booking.AcceptBooking;

namespace PadelApp.Application.Commands.Booking.RejectBooking;

public record RejectBookingCommand(Guid BookingId, Guid CourtId) : IRequest<RejectBookingResponse>;