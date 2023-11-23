namespace PadelApp.Application.Abstractions.Emai;

public interface INotificationService
{
    Task NotifyUserAsync(Guid userId, string message);
    Task NotifyUserForBookingAcceptance(Guid userId, Guid bookingId);
}