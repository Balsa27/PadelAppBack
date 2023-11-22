using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Google;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Enums;

namespace PadelApp.Application.Commands.Player.GoogleSignIn;

public class GoogleSignInRequestHandler : IRequestHandler<GoogleSignInCommand, Result<GoogleSignInResponse>>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IGoogleTokenValidator _googleTokenValidator;

    public GoogleSignInRequestHandler(
        IPlayerRepository playerRepository,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IGoogleTokenValidator googleTokenValidator)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _googleTokenValidator = googleTokenValidator;
    }

    public async Task<Result<GoogleSignInResponse>> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
    {
        var googleUserPayload = await _googleTokenValidator
            .ValidateAsync(request.GoogleToken);

        var existingPlayer = await _playerRepository.GetByGoogleId(googleUserPayload.Subject);

        Domain.Aggregates.Player? newPlayer = null;

        if (existingPlayer is null)
        {
            newPlayer = new Domain.Aggregates.Player(
                googleUserPayload.GivenName, 
                string.Empty,            
                googleUserPayload.Email,  
                googleUserPayload.Subject,
                null                
            );
            
            await _playerRepository.AddAsync(newPlayer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        var token = _jwtProvider.GeneratePlayerToken(existingPlayer ?? newPlayer!);

        return Result<GoogleSignInResponse>
            .Success(new GoogleSignInResponse(googleUserPayload.Subject, googleUserPayload.Email, token));
    }
}