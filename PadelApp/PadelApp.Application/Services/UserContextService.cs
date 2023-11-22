using System.Security.Claims;
using DrealStudio.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DrealStudio.Application.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        
        if (user?.Identity != null && (user == null || !user.Identity.IsAuthenticated))
            throw new UnauthorizedAccessException("User is not authenticated.");

        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
        
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        throw new UnauthorizedAccessException("User ID is missing from token.");
    }
}