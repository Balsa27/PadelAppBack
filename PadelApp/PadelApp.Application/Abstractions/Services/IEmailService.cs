namespace PadelApp.Application.Abstractions.Emai;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string username);
}