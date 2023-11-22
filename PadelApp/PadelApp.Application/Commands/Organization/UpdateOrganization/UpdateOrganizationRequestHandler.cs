using DrealStudio.Application.Services.Interface;
using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Application.Strings;

namespace PadelApp.Application.Commands.Organization.UpdateOrganization;

public class UpdateOrganizationRequestHandler : IRequestHandler<UpdateOrganizationCommand, UpdateOrganizationResponse>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public UpdateOrganizationRequestHandler(
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<UpdateOrganizationResponse> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var id = _userContextService.GetCurrentUserId();

        var organization = await _organizationRepository.GetByIdAsync(id);

        if (organization is null)
            throw new OrganizationNotFoundException("Organization not found");
        
        organization.UpdateDetails(
            request.Name,
            request.Description,
            request.Address,
            request.ContactInfo,
            request.WorkingStartHours,
            request.WorkingEndingHours);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateOrganizationResponse(HandlerStrings.OrganizationUpdated);
    }
}