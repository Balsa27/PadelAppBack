using PadelApp.Domain.Enums;

namespace PadelApp.Application.Commands.Auth.Login;

public record UserLoginResponse(string Token, Role Role);
