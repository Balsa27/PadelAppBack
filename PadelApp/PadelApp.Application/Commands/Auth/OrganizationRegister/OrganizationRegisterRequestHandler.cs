using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Authentication;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;

namespace PadelApp.Application.Commands.Organization.OrganizationRegister;

public class OrganizationRegisterRequestHandler : IRequestHandler<OrganizationRegisterCommand, OrganizationRegisterResponse>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationRegisterRequestHandler(
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
    }

    public async Task<OrganizationRegisterResponse> Handle(OrganizationRegisterCommand request, CancellationToken cancellationToken)
    {
        var existingOrganizationResult = await _organizationRepository
            .GetByUsernameOrEmail(request.Username, request.Email);
        
        if (existingOrganizationResult is not null)
            throw new OrganizationAlreadyExistsException("Organization with the same username or email already exists");
        
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var organization = new Domain.Entities.Organization(
            request.Username,
            request.Email,
            hashedPassword,
            request.Name,
            request.Description,
            request.Address,
            request.ContactInfo,
            request.Start,
            request.End);
        
        await _organizationRepository.Add(organization);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var token = _jwtProvider.GenerateOrganizationToken(organization);
        
        return new OrganizationRegisterResponse(
            organization.Id,
            request.Username,
            request.Email,
            request.Name,
            request.Description,
            request.Address,
            request.ContactInfo,
            request.Start,
            request.End,
            organization.Status,
            organization.Role,
            token);
    }
}