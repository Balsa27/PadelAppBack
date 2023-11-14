using MediatR;
using PadelApp.Application.Abstractions.Emai;
using PadelApp.Domain.Events.DomainEvents.DomainEventConverter;

namespace PadelApp.Application.Events.Domain;

public class BookingRejectedEventHandler : INotificationHandler<BookingRejectedDomainEvent>
{
    private readonly INotificationService _notificationService;

    public BookingRejectedEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(BookingRejectedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notificationService.NotifyUserAsync(notification.BookerId, "Your booking has been rejected.");
    }
}