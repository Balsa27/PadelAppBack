using MediatR;
using PadelApp.Application.Abstractions.Emai;
using PadelApp.Domain.Events.DomainEvents;

namespace PadelApp.Application.Events.Domain;

public class BookingCancelledDomainEventHandler : INotificationHandler<BookingCancelledDomainEvent>
{
    private readonly INotificationService _notificationService;

    public BookingCancelledDomainEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(BookingCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        //await _notificationService.NotifyUserAsync(notification.BookerId, "Your booking has been cancelled!");
    }
}