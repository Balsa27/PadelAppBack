using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Apple;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.ErrorHandling;

namespace PadelApp.Application.Commands.Player.AppleSignIn;

public class AppleSignInRequestHandler : IRequestHandler<AppleSignInCommand, Result<AppleSignInResponse>>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppleSignInRequestHandler(
        IPlayerRepository playerRepository,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<AppleSignInResponse>> Handle(AppleSignInCommand request, CancellationToken cancellationToken)
    {   
        var userIdClaim =  _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        var emailClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email);
        Domain.Aggregates.Player? newPlayer = null;
        
        if (userIdClaim is null || emailClaim is null)
            return Result<AppleSignInResponse>.Fail(DomainErrors.UserNotFound());
        
        var existingPlayer = await _playerRepository.GetByAppleId(userIdClaim.Value);
        
        if (existingPlayer is null)
        {
            newPlayer = new Domain.Aggregates.Player(
                emailClaim.Value,
                string.Empty,
                emailClaim.Value,
                null,
                userIdClaim.Value);
            
            await _playerRepository.AddAsync(newPlayer);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        };
        
        var token = _jwtProvider.GeneratePlayerToken(existingPlayer ?? newPlayer!);
        
        return Result<AppleSignInResponse>
            .Success(new AppleSignInResponse(userIdClaim.Value, emailClaim.Value, token));
    }
}