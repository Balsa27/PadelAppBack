// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using Microsoft.IdentityModel.Tokens;
// using PadelApp.Application.Abstractions.Apple;
// using PadelApp.Application.Commands.Player.AppleSignIn;
//
// namespace PadelApp.Infrastructure.Authentication.Apple;
//
// public class AppleTokenValidator : IAppleTokenValidator
// {
//     public Task<AppleUserPayload> ValidateAsync(string appleToken)
//     {
//         var TokenValidationParams = new TokenValidationParameters
//         {
//             ValidIssuer = "https://appleid.apple.com",
//             ValidAudience = "Your Apple Client ID Here",
//             IssuerSigningKey = new RsaSecurityKey(),
//             ValidateIssuerSigningKey = true,
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ClockSkew = TimeSpan.Zero
//         };
//
//         var tokenHandler = new JwtSecurityTokenHandler();
//         
//         var principal = tokenHandler.ValidateToken(appleToken, TokenValidationParams, out var validatedToken);
//         
//         var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
//         var emailClaim = principal.FindFirst(ClaimTypes.Email);
//         
//         if (userIdClaim == null || emailClaim == null)
//             throw new Exception("Required claims are missing");
//         
//         return Task.FromResult(new AppleUserPayload(userIdClaim.Value, emailClaim.Value));
//     }
// }