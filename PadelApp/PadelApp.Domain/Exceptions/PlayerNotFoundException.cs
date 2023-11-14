namespace PadelApp.Application.Exceptions;

public class PlayerNotFoundException : Exception
{
    public PlayerNotFoundException(string message) : base(message) {}
}