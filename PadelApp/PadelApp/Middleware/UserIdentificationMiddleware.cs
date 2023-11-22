using System.Security.Claims;

namespace PadelApp.Middleware;

public class UserIdentificationMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdentificationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is { IsAuthenticated: true })
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
                context.Items["UserId"] = userId;
        }

        await _next(context);
    }
}