using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PadelApp.Domain.Enums;

namespace PadelApp.Presentation.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class TokenAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly Role _requiredRole;
    
    public TokenAttribute(Role role) : base(role == Role.Player ? "Player" : "Organization")
    {
        _requiredRole = role;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity is { IsAuthenticated: false })
            throw new UnauthorizedAccessException("User is not authenticated.");
            
        var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
        
        if (roleClaim == null || !Enum.TryParse(roleClaim, out Role userRole) || userRole != _requiredRole)
            throw new AccessViolationException("User does not have the required role.");
    }
}