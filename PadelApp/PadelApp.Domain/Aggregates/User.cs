using PadelApp.Domain.Enums;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Aggregates;

public abstract class User : AggregateRoot
{
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public Role Role { get; private set; }
    public string? GoogleId { get; private set; }
    public string? AppleId { get; private set; }

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
        Role = Role.USER;
        GoogleId = googleId;
        AppleId = appleId;//za sad
    }
}