using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.ValueObjects;

public class Team : ValueObject
{
    public Guid Player1Id { get; private set; }
    public Guid Player2Id { get; private set; }
    
    public Team(Guid player1Id, Guid player2Id)
    {
        Player1Id = player1Id;
        Player2Id = player2Id;
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Player1Id;
        yield return Player2Id;
    }
}