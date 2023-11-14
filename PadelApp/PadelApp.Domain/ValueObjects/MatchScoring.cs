using PadelApp.Domain.Aggregates;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.ValueObjects;

public class MatchScoring : ValueObject
{
    public List<SetScoring> Sets { get; private set; } = new();
    public int TeamOneMatchWins { get; private set; } = 0;
    public int TeamTwoMatchWins { get; private set; } = 0;
    public bool IsOver { get; private set; } = false;
    
    public MatchScoring()
    {
        StartNewSet();
    }

    private void StartNewSet() => Sets.Add(new SetScoring());

    public void AddPointToPlayer(Guid playerId, List<Guid> teamOne, List<Guid> teamTwo)
    {
        var currentSet = Sets[^1]; //last set

        if (currentSet.IsOver)
        {
            if(currentSet.TeamOneSetScore > currentSet.TeamTwoSetScore)
                TeamOneMatchWins++;
            else
                TeamTwoMatchWins++;
            
            if (TeamOneMatchWins == 2 || TeamTwoMatchWins == 2)
                IsOver = true;
            
            StartNewSet();
            currentSet = Sets[^1]; //update to the new last set
        }
        
        currentSet.AddSetPoint(playerId, teamOne, teamTwo);
    }
        
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Sets;
        yield return TeamOneMatchWins;
        yield return TeamTwoMatchWins;
        yield return IsOver;
    }
}