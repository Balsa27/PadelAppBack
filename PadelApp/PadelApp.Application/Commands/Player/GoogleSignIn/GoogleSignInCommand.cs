using MediatR;

namespace PadelApp.Application.Commands.Player.GoogleSignIn;

public record GoogleSignInCommand(string GoogleToken) : IRequest<Result<GoogleSignInResponse>>;
