using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.ErrorHandling;

namespace PadelApp.Application.Commands.Player.Login;

public class PlayerLoginRequestHandler : IRequestHandler<PlayerLoginCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly IJwtProvider _jwtProvider;

    public PlayerLoginRequestHandler(
        IUnitOfWork unitOfWork,
        IPlayerRepository playerRepository,
        IJwtProvider jwtProvider)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string>> Handle(PlayerLoginCommand request, CancellationToken cancellationToken)
    {
        var existingPlayerResult = await _playerRepository
            .GetByUsernameOrEmail(request.UsernameOrEmail, request.UsernameOrEmail);

        if (existingPlayerResult is null)
            return Result<string>.Fail(DomainErrors.InvalidCredentials());

        bool verified = BCrypt.Net.BCrypt.Verify(request.Password, existingPlayerResult.Password);

        if (!verified)
            return Result<string>.Fail(DomainErrors.InvalidCredentials());

        var token = _jwtProvider.GeneratePlayerToken(existingPlayerResult);

        return Result<string>.Success(token);
    }
}