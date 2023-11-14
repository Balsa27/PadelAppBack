namespace PadelApp.Application.Exceptions;

public class CourtAlreadyExistsException : Exception
{
    public CourtAlreadyExistsException(string message) : base(message) { }
}