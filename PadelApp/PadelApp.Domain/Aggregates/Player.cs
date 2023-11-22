using PadelApp.Domain.DomainEvents;
using PadelApp.Domain.Enums;

namespace PadelApp.Domain.Aggregates;

public class Player : User
{
    public Player(string username, string password, string email) 
        : base(username, password, email)
    {
        Role = Role.Player;
        RaiseDomainEvent(new PlayerRegisteredDomainEvent(Id, username, email));
    }
    
    public Player(string username, string password, string email, string? googleId, string? appleId)
        : base(username, password, email, googleId, appleId)
    {
        RaiseDomainEvent(new PlayerRegisteredDomainEvent(Id, username, email));
    }

    public Player() : base()
    {
        
    }
}