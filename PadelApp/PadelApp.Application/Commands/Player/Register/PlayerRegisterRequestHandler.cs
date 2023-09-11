using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.ErrorHandling;

namespace PadelApp.Application.Handlers;

public class PlayerRegisterRequestHandler : IRequestHandler<PlayerRegisterCommand, Result<string>>
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

    public async Task<Result<string>> Handle(PlayerRegisterCommand request, CancellationToken cancellationToken)
    {
        var existingPlayerResult = await _playerRepository
            .GetByUsernameOrEmail(request.Username, request.Email);

        if (existingPlayerResult is not null)
            return Result<string>.Fail(DomainErrors.UserAlreadyExists());
        
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var player = new Player(request.Username, hashedPassword, request.Email);
        
        await _playerRepository.AddAsync(player);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtProvider.GeneratePlayerToken(player);

        return Result<string>.Success(token);
    }
}