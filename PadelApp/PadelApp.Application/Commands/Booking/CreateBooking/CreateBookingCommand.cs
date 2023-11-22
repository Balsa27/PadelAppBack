using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Domain.Entities;

namespace PadelApp.Application.Commands.Player.CreateBooking;

public record CreateBookingCommand(
    Guid CourtId,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime) : IRequest<CreateBookingResponse>;
