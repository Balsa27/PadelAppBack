using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Domain.Entities;

namespace PadelApp.Application.Commands.Player.CreateBooking;

public record CreateBookingCommand(
    Guid CourtId,
    string CortName,
    Guid BookerId,
    DateTime StartTime,
    DateTime EndTime) : IRequest<CreateBookingResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
