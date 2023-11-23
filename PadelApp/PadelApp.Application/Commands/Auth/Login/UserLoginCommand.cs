using MediatR;
using PadelApp.Application.Commands.Auth.Login;

namespace PadelApp.Application.Commands.Player.Login;

public record UserLoginCommand(string UsernameOrEmail, string Password) : IRequest<UserLoginResponse>;