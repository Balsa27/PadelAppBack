namespace PadelApp.Application.Exceptions;

public class OrganizationAlreadyExistsException : Exception
{
    public OrganizationAlreadyExistsException(string message) : base(message)
    {
        
    }
}