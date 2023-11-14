using Google.Apis.Auth;
using PadelApp.Application.Abstractions.Google;
using PadelApp.Application.Commands.Player.GoogleSignIn;

namespace PadelApp.Infrastructure.Authentication.Google;

public class GoogleTokenValidator : IGoogleTokenValidator
{
    public async Task<GoogleUserPayload> ValidateAsync(string token)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(token);
        return new GoogleUserPayload(payload.Subject, payload.Email, payload.GivenName);
    }
}