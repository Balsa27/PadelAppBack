using MediatR;
using PadelApp.Application.Abstractions.Emai;
using PadelApp.Domain.Events.DomainEvents;

namespace PadelApp.Application.Events.Domain;

public class BookingCreatedDomainEventHandler : INotificationHandler<BookingCreatedDomainEvent>
{
    private readonly INotificationService _notificationService;

    public BookingCreatedDomainEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(BookingCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notificationService.NotifyUserAsync(notification.OrganizationId, "You have a new pending booking!");
    }
}