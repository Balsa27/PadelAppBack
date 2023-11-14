using PadelApp.Domain.Enums;
using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.ValueObjects;

public class GameScoring : ValueObject
{
    public List<Point> Points { get; private set; } = new();
    public GamePoint TeamOneScore { get; private set; }
    public GamePoint TeamTwoScore { get; private set; }
    public int TeamOneTieBreakScore { get; private set; }
    public int TeamTwoTieBreakScore { get; private set; }
    public bool IsTieBreak { get; private set; }
    public bool IsOver { get; private set; }
    

    public GameScoring(bool isTieBreak = false)
    {
        IsTieBreak = isTieBreak;
        TeamOneScore = GamePoint.Zero;
        TeamTwoScore = GamePoint.Zero;
    }

    public void AddPoint(Guid playerId, List<Guid> teamOne, List<Guid> teamTwo)
    {
        if (IsOver)
            throw new InvalidOperationException("The game is already over.");
        
        Points.Add(new Point(playerId, DateTime.UtcNow));

        if (IsTieBreak)
                UpdateTieBreakScore(playerId, teamOne, teamTwo);
        else
        {
            if (teamOne.Contains(playerId))
                (TeamOneScore, TeamTwoScore) = UpdateScore(TeamOneScore, TeamTwoScore);
            else if (teamTwo.Contains(playerId))
                (TeamTwoScore, TeamOneScore) = UpdateScore(TeamTwoScore, TeamOneScore);
        }
        
        throw new ArgumentException("Player ID not found in either team");
    }

    public void StartTieBreak()
    {
        IsTieBreak = true;
        TeamOneTieBreakScore = 0;
        TeamTwoTieBreakScore = 0;
    }

    (GamePoint, GamePoint) UpdateScore(GamePoint teamScore, GamePoint opponentScore)
    {
        if(IsOver)
            throw new InvalidOperationException("The game is already over.");
        switch (teamScore)
        {
            case GamePoint.Zero:
                teamScore = GamePoint.Fifteen;
                break;
            case GamePoint.Fifteen:
                teamScore = GamePoint.Thirty;
                break;
            case GamePoint.Thirty:
                teamScore = GamePoint.Forty;
                break;
            case GamePoint.Forty:
                if (opponentScore < GamePoint.Forty)
                {
                    teamScore = GamePoint.GameWin;
                    IsOver = true;
                }
                else if (opponentScore == GamePoint.Forty)
                    teamScore = GamePoint.Advantage;
                else if (opponentScore == GamePoint.Advantage)
                    opponentScore = GamePoint.Forty;
                break;
            case GamePoint.Advantage:
                if (opponentScore == GamePoint.Forty)
                {
                    teamScore = GamePoint.GameWin;
                    IsOver = true;
                }
                else if (opponentScore == GamePoint.Advantage)
                    throw new InvalidOperationException("Both teams can't have advantage at the same time");
                break;
            default:
                throw new InvalidOperationException("Game is already over");
        }
        return (teamScore, opponentScore);
    }

    private void UpdateTieBreakScore(Guid playerId, List<Guid> teamOne, List<Guid> teamTwo)
    {
        if(teamOne.Contains(playerId))
            TeamOneTieBreakScore++;
        else if (teamTwo.Contains(playerId))
            TeamTwoTieBreakScore++;
        
        if(TeamOneTieBreakScore >= 7 &&
           TeamTwoTieBreakScore <= TeamOneTieBreakScore - 2)
        {
            TeamOneScore = GamePoint.GameWin;
            IsOver = true;
        }
        else if (TeamTwoTieBreakScore >= 7 &&
                 TeamOneTieBreakScore <= TeamTwoTieBreakScore - 2)
        {
            TeamTwoScore = GamePoint.GameWin;
            IsOver = true;
        }
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Points;
        yield return TeamOneScore;
        yield return TeamTwoScore;
        yield return TeamOneTieBreakScore;
        yield return TeamTwoTieBreakScore;
        yield return IsTieBreak;
        yield return IsOver;
    }
}