using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Domain.Events.DomainEvents;

namespace PadelApp.Application.Events.Domain;

public class RemoveCourtFromOrganizationDomainEventHandler : INotificationHandler<RemoveCourtFromOrganizationDomainEvent>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCourtFromOrganizationDomainEventHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveCourtFromOrganizationDomainEvent notification, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(notification.OrganizationId);
        
        if (organization == null)
            throw new OrganizationNotFoundException("Organization not found");
        
        organization.RemoveCourt(notification.CourtId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}