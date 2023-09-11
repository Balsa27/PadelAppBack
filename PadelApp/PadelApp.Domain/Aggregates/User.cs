using PadelApp.Domain.Enums;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.Aggregates;

public abstract class User : Entity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    protected User() { }
    
    protected User(
        string username,
        string password,
        string email)
    {
        Username = username;
        Password = password;
        Email = email;
        Role = Role.USER; //za sad
    }
}