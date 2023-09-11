using Microsoft.AspNetCore.Authorization;
using PadelApp.Domain.Enums;

namespace PadelApp.Presentation.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class TokenAttribute : AuthorizeAttribute
{
    public TokenAttribute(Role role) : base($"Token{role.ToString()}")
    {
    }
}