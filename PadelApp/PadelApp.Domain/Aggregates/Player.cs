namespace PadelApp.Domain.Aggregates;

public class Player : User
{
    public Player(string username, string password, string email) 
        : base(username, password, email)
    {
        
    }

    public Player() : base()
    {
        
    }
}