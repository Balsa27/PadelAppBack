namespace PadelApp.Application.Exceptions;

public class OrganizationNotFoundException : Exception
{
    public OrganizationNotFoundException(string message) : base(message)
    {
        
    }
}