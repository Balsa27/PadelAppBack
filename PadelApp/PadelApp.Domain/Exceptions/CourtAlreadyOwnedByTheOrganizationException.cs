namespace PadelApp.Application.Exceptions;

public class CourtAlreadyOwnedByTheOrganizationException : Exception
{
    public CourtAlreadyOwnedByTheOrganizationException(string message) : base(message)
    {
    }    
}