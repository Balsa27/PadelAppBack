namespace PadelApp.Domain.ErrorHandling;

public static class DomainErrors
{
    public static Error UserAlreadyExists() =>
        new Error($"USER_ALREADY_EXISTS", "User already exists");
}