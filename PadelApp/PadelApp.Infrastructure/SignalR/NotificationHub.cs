using System.IdentityModel.Tokens.Jwt;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace PadelApp.Infrastructure.SignalR;

public class NotificationHub : Hub
{
    static ConcurrentDictionary<string, string> _users = new();
    
    public override Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        if (httpContext is not null)
        {
            var userId = httpContext.Request.Query["userId"];
            _users.TryAdd(Context.ConnectionId, userId);
        }
        
        return base.OnConnectedAsync();
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _users.TryRemove(Context.ConnectionId, out _);   
        return base.OnDisconnectedAsync(exception);
    }
}