using PadelApp.Domain.Enums;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.ValueObjects;

public class SetScoring : ValueObject
{
    public List<GameScoring> Games { get; private set; } = new();
    public int TeamOneSetScore { get; private set; } = 0;
    public int TeamTwoSetScore { get; private set; } = 0;
    public bool IsOver { get; private set; } = false;

    public SetScoring()
    {
        StartNewGame();
    }

    public void StartNewGame(bool isTieBreak = false)
    {
        if (IsOver)
            throw new InvalidOperationException("The set is already over.");
        var newGame = new GameScoring(isTieBreak);
    }

    public void AddSetPoint(Guid playerId, List<Guid> teamOne, List<Guid> teamTwo)
    {
        var currentGame = Games[^1]; //last game

        if (currentGame.IsOver)
        {
            if (currentGame.TeamOneScore == GamePoint.GameWin)
                TeamOneSetScore++;
            TeamTwoSetScore++;

            if ((TeamOneSetScore >= 6 || TeamTwoSetScore >= 6) &&
                Math.Abs(TeamOneSetScore - TeamTwoSetScore) >= 2)
                IsOver = true;
            else if (TeamOneSetScore == 6 && TeamTwoSetScore == 6)
                StartNewGame(isTieBreak: true);
        }
        StartNewGame();
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Games;
        yield return TeamOneSetScore;
        yield return TeamTwoSetScore;
        yield return IsOver;
    }
}