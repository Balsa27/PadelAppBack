using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Commands.Player.Login;
using PadelApp.Domain.ErrorHandling;

namespace PadelApp.Application.Commands.Auth.Login;

public class UserLoginRequestHandler : IRequestHandler<UserLoginCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IJwtProvider _jwtProvider;

    public UserLoginRequestHandler(
        IUnitOfWork unitOfWork,
        IPlayerRepository playerRepository,
        IJwtProvider jwtProvider, 
        IOrganizationRepository organizationRepository)
    {
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        _jwtProvider = jwtProvider;
        _organizationRepository = organizationRepository;
    }

    public async Task<Result<string>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _playerRepository
            .GetByUsernameOrEmail(request.UsernameOrEmail, request.UsernameOrEmail);
        
        if(user is not null)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            
            if (!verified)
                return Result<string>.Fail(DomainErrors.InvalidCredentials());

            var token = _jwtProvider.GeneratePlayerToken(user);
            
            return Result<string>.Success(token);
        }
        
        var organization = await _organizationRepository
            .GetByUsernameOrEmail(request.UsernameOrEmail, request.UsernameOrEmail);

        if (organization is not null)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, organization.Password);
            
            if (!verified)
                return Result<string>.Fail(DomainErrors.InvalidCredentials());

            var token = _jwtProvider.GenerateOrganizationToken(organization);
            
            return Result<string>.Success(token);
        }

        return Result<string>.Fail(DomainErrors.InvalidCredentials());
    }
}