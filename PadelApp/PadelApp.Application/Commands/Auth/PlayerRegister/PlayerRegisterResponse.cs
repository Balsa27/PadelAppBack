using PadelApp.Domain.Enums;

namespace PadelApp.Application.Handlers;

public record PlayerRegisterResponse(string Token, Guid Id, Role Role, string Username, string Email);