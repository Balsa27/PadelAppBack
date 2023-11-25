using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.ErrorHandling;

namespace PadelApp.Application.Handlers;

public class PlayerRegisterRequestHandler : IRequestHandler<PlayerRegisterCommand, PlayerRegisterResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPlayerRepository _playerRepository;

    public PlayerRegisterRequestHandler(
        IUnitOfWork unitOfWork,
        IPlayerRepository playerRepository, 
        IJwtProvider jwtProvider)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<PlayerRegisterResponse> Handle(PlayerRegisterCommand request, CancellationToken cancellationToken)
    {
        var existingPlayerResult = await _playerRepository
            .GetByUsernameOrEmail(request.Username, request.Email);

        if (existingPlayerResult is not null)
            throw new PlayerNotFoundException("Player with the same username or email already exists");        
       
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var player = new Player(request.Username, hashedPassword, request.Email);

        await _playerRepository.AddAsync(player);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var token = _jwtProvider.GeneratePlayerToken(player);

        return new PlayerRegisterResponse(
            token,
            player.Id,
            player.Role,
            player.Username,
            player.Email);
    }
}