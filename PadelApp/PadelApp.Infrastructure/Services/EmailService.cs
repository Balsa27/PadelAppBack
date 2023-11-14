using Microsoft.Extensions.Configuration;
using PadelApp.Application.Abstractions.Emai;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PadelApp.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendWelcomeEmailAsync(string email, string username)
    {
        var client = new SendGridClient(_configuration.GetSection("SendGrid")["ApiKey"]);
        var from = new EmailAddress("padelappmontenegro@gmail.com");
        var to = new EmailAddress(email);
        var subject = "Welcome to the PadelApp!";
        var plainTextContent = $"Hello, {username}!";
        var htmlContent = $"<strong>Hello, {username}!</strong> Welcome to our application.";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        await client.SendEmailAsync(msg);
    }
}