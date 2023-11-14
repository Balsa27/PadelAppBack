using MediatR;

namespace PadelApp.Application.Commands.Player.AppleSignIn;

public record AppleSignInCommand(string AppleToken) : IRequest<Result<AppleSignInResponse>>;
