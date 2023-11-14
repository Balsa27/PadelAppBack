namespace PadelApp.Application.Abstractions.Emai;

public interface INotificationService
{
    Task NotifyUserAsync(Guid userId, string message);
}