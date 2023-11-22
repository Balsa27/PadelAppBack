using PadelApp.Domain.Enums;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Aggregates;

public abstract class User : AggregateRoot
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public Role Role { get; protected set; }
    public string? GoogleId { get; private set; }
    public string? AppleId { get; private set; }
    public bool IsActive => isActive;
    private bool isActive; 

    protected User() { }
    
    protected User(
        string username,
        string password,
        string email,
        string? googleId = null,
        string? appleId = null)
    {
        Username = username;
        Password = password;
        Email = email;
        GoogleId = googleId;
        AppleId = appleId;//za sad
        isActive = true;
    }

    protected User(
        string username,
        string password,
        string email)
    {
        Username = username;
        Password = password;
        Email = email;
        isActive = true;
    }
    
    public void Deactivate()
    {
        isActive = false;
    }
}