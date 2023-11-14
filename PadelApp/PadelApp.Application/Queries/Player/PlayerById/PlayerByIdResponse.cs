namespace PadelApp.Application.Queries.Player.PlayerById;

public record PlayerByIdResponse(
    Guid PlayerId,
    string Username,
    string Password,
    string Email,
    string? GoogleId,
    string? AppleId);