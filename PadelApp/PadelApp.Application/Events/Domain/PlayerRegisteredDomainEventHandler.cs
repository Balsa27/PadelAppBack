using MediatR;
using PadelApp.Application.Abstractions.Emai;
using PadelApp.Domain.DomainEvents;

namespace PadelApp.Application.Events.Domain;

public class PlayerRegisteredDomainEventHandler : INotificationHandler<PlayerRegisteredDomainEvent>, INotification
{
    private readonly IEmailService _email;

    public PlayerRegisteredDomainEventHandler(IEmailService email)
    {
        _email = email;
    }

    public Task Handle(PlayerRegisteredDomainEvent notification, CancellationToken cancellationToken) 
    {
        return _email.SendWelcomeEmailAsync(notification.Email, notification.Username);
    }
}