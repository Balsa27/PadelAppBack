using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Application.Strings;

namespace PadelApp.Application.Commands.Organization.RemoveOrganization;

public class RemoveOrganizationRequestHandler : IRequestHandler<RemoveOrganizationCommand, RemoveOrganizationResponse>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public RemoveOrganizationRequestHandler(
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<RemoveOrganizationResponse> Handle(RemoveOrganizationCommand request, CancellationToken cancellationToken)
    {
        var id = _userContextService.GetCurrentUserId();
        
        var organization = await _organizationRepository.GetByIdAsync(id);

        if (organization is null)
            throw new OrganizationNotFoundException("Organization not found");
        
        organization.Deactivate();
        
        _organizationRepository.Update(organization);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new RemoveOrganizationResponse(HandlerStrings.OrganizationRemoved);
    }
}