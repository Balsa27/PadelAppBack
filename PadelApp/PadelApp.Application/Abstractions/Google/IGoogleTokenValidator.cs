using PadelApp.Application.Commands.Player.GoogleSignIn;

namespace PadelApp.Application.Abstractions.Google;

public interface IGoogleTokenValidator
{
    Task<GoogleUserPayload> ValidateAsync(string token);
}