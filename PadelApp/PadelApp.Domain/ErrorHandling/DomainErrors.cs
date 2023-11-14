namespace PadelApp.Domain.ErrorHandling;

public static class DomainErrors
{
    public static Error UserAlreadyExists() =>
        new Error($"USER_ALREADY_EXISTS", "User already exists");
    
    public static Error UserNotFound() =>
        new Error($"USER_NOT_FOUND", "User not found");

    public static Error InvalidCredentials() =>
        new Error($"INVALID_CREDENTIALS", "INVALID_CREDENTIALS");
}