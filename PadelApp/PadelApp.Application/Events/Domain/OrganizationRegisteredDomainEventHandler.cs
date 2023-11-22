using MediatR;
using PadelApp.Application.Abstractions.Emai;

namespace PadelApp.Application.Events.Domain;

public class OrganizationRegisteredDomainEventHandler : INotificationHandler<OrganizationRegisteredDomainEvent>
{
    private readonly IEmailService _emailService;

    public OrganizationRegisteredDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public  Task Handle(OrganizationRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        return _emailService.SendWelcomeEmailAsync(notification.Email, notification.Name);
    }
}