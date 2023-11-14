namespace PadelApp.Application.Commands.Player.GoogleSignIn;

public record GoogleSignInResponse(string UserId, string Email, string JwtToken);
