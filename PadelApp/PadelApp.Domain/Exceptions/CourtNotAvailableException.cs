namespace PadelApp.Application.Exceptions;

public class CourtNotAvailableException : Exception
{
    public CourtNotAvailableException(string message) : base(message) { }
}