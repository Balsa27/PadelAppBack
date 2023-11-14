using PadelApp.Application.Commands.Player.AppleSignIn;

namespace PadelApp.Application.Abstractions.Apple;

public interface IAppleTokenValidator
{
    Task<AppleUserPayload> ValidateAsync(string appleToken);
}