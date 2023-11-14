using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Domain.ValueObjects;

namespace PadelApp.Application.Commands.Court.AddCourt;

public record AddCourtCommand(
    string Name,
    string Description,
    Address Address,
    DateTime WorkStartTime,
    DateTime WorkEndTime,
    Price Price) : IRequest<AddCourtResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}