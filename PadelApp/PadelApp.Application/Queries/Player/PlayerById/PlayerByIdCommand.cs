using DrealStudio.Application.Services.Interface;
using MediatR;

namespace PadelApp.Application.Queries.Player.PlayerById;

public record PlayerByIdCommand() : IRequest<PlayerByIdResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
