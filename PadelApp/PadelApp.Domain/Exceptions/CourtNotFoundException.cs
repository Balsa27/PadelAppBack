namespace PadelApp.Application.Exceptions;

public class CourtNotFoundException : Exception
{
    public CourtNotFoundException(string message) : base(message) { }
}