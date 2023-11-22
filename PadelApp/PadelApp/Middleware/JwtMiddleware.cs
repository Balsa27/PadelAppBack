using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace PadelApp.Middleware;

public class JwtMiddleware 
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }
    
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();
        
        if (token != null)
            AttachUserToContext(context, token);

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]!);

            // Validate the token and get the security token
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // Optionally validate issuer if required
                ValidateAudience = false, // Optionally validate audience if required
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            context.User = principal;
        }
        catch (Exception)
        {
            // Do nothing if the token validation fails
            // The user isn't attached to the HttpContext so the endpoint will be inaccessible
        }
    }
}