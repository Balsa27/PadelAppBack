using MediatR;

namespace PadelApp.Application.Commands.Player.Login;

public record PlayerLoginCommand(string UsernameOrEmail, string Password) : IRequest<Result<string>>;