using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using PadelApp.Application.Abstractions.Emai;
using PadelApp.Infrastructure.SignalR;

namespace PadelApp.Infrastructure.Email;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyUserAsync(Guid userId, string message)
    {
        var connectionId = NotificationHub._users.FirstOrDefault(u => u.Value == userId.ToString()).Key;
       
        if (string.IsNullOrEmpty(connectionId)) return;

        if (!string.IsNullOrEmpty(connectionId))
        {
            await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }
    }
    public async Task NotifyUserForBookingAcceptance(Guid userId, Guid bookingId)
    {
        var connectionId = NotificationHub._users.FirstOrDefault(u => u.Value == userId.ToString()).Key;

        if (!string.IsNullOrEmpty(connectionId))
        {
            var message = new
            {
                Action = "BookingOffer",
                BookingId = bookingId,
                Message = "A booking slot is available. Do you want to accept it?"
            };

            await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }
    }
}