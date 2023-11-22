using MediatR;
using PadelApp.Application.Abstractions;
using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Application.Exceptions;
using PadelApp.Domain.Entities;
using PadelApp.Domain.Events.DomainEvents;

namespace PadelApp.Application.Events.Domain;

public class CourtCreatedDomainEventHandler : INotificationHandler<CourtCreatedDomainEvent>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CourtCreatedDomainEventHandler(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CourtCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(notification.OrganizationId);
        
        if (organization == null)
            throw new OrganizationNotFoundException("Organization not found");
        
        var organizationCourt = new OrganizationCourt(notification.OrganizationId, notification.CourtId);
        
        organization.AddCourt(organizationCourt);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}