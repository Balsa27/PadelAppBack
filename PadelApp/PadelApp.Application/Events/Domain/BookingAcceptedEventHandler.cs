using MediatR;
using PadelApp.Application.Abstractions.Emai;
using PadelApp.Domain.Events.DomainEvents.DomainEventConverter;

namespace PadelApp.Application.Events.Domain;

public class BookingAcceptedEventHandler : INotificationHandler<BookingAcceptedDomainEvent>
{
    private readonly INotificationService _notificationService;

    public BookingAcceptedEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(BookingAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notificationService.NotifyUserAsync(notification.BookerId, "Your booking has been accepted!");
    }
}