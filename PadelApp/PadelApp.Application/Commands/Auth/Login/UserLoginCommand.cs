using MediatR;

namespace PadelApp.Application.Commands.Player.Login;

public record UserLoginCommand(string UsernameOrEmail, string Password) : IRequest<Result<string>>;