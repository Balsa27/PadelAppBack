using MediatR;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;

namespace PadelApp.Application.Queries.Player.PlayerById;

public class PlayerByIdRequestHandler : IRequestHandler<PlayerByIdCommand, PlayerByIdResponse>
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerByIdRequestHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<PlayerByIdResponse> Handle(PlayerByIdCommand request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetById(request.UserId);
        
        if (player is null)
            throw new PlayerNotFoundException($"Player with id {request.UserId} not found");

        return new PlayerByIdResponse(
            player.Id,
            player.Username,
            player.Password,
            player.Email,
            player.GoogleId,
            player.AppleId
            );
    }
}