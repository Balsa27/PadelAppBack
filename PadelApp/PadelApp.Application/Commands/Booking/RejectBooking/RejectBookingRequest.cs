using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Commands.Booking.AcceptBooking;

namespace PadelApp.Application.Commands.Booking.RejectBooking;

public record RejectBookingRequest(Guid BookingId, Guid BookerId, Guid CourtId);
