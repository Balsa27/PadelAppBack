using PadelApp.Domain.Enums;
using PadelApp.Domain.Primitives;
using PadelApp.Domain.ValueObjects;
using MatchType = PadelApp.Domain.Enums.MatchType;

namespace PadelApp.Domain.Aggregates;

public class Match : AggregateRoot
{
    public Guid CourtId { get; private set; }
    public Guid AdminId { get; private set; }
    public List<Guid> TeamOne { get; private set; }
    public List<Guid> TeamTwo { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public MatchStatus Status { get; private set; }
    public MatchType Type { get; private set; } 
    public MatchScoring Score { get; private set; }

    private readonly int _maxPlayers  = 4;
    private readonly int _minPlayers = 0;

    
    public Match(
        Guid courtId,
        List<Guid> teamOne,
        List<Guid> teamTwo,
        MatchStatus status,
        MatchType type)
    {
        if (teamOne.Count > _maxPlayers || teamTwo.Count > _maxPlayers)
            throw new ArgumentException("Too many players");
        
        CourtId = courtId;
        Status = status;
        Type = type;
        TeamOne = teamOne;
        TeamTwo = teamTwo;
    }

    public void SetAdmin(Guid adminId)
    {
        AdminId = adminId;
    }

    public void AddPlayerToTeam(Guid playerId, bool isTeamOne)
    {
        if (Status == MatchStatus.Started)
            throw new InvalidOperationException("Cannot add players after the match has started");

        var team = isTeamOne ? TeamOne : TeamTwo;
        
        if(team.Count >= 2)
            throw new InvalidOperationException("Team is already full");
        
        if(team.Contains(playerId))
            throw new InvalidOperationException("Player is already in the team");
        
        team.Add(playerId);
        UpdateMatchTypeAndStatus();
    }
    
    public void RemovePlayerFromTeam(Guid playerId, bool isTeamOne)
    {
        if (Status == MatchStatus.Started)
            throw new InvalidOperationException("Cannot remove players after the match has started");

        var team = isTeamOne ? TeamOne : TeamTwo;
        
        if(team.Count <= 0)
            throw new InvalidOperationException("Team is already empty");
        
        if(!team.Contains(playerId))
            throw new InvalidOperationException("Player is not in the team");
        
        team.Remove(playerId);
        UpdateMatchTypeAndStatus();
    }
    
    public void StartMatch(Guid requestorId)
    {
        if (AdminId != requestorId)
            throw new InvalidOperationException("Only the admin can start the match");
        
        if (Status == MatchStatus.Started)
            throw new InvalidOperationException("Match has already started");
        
        if(Type == MatchType.Single &&
           (TeamOne.Count != 1 || TeamTwo.Count != 1))
            throw new InvalidOperationException("Single match must have one player per team");
        
        if(Type == MatchType.Double &&
           (TeamOne.Count != 2 || TeamTwo.Count != 2))
            throw new InvalidOperationException("Double match must have two players per team");
        
        Status = MatchStatus.Started;
        StartTime = DateTime.UtcNow;
        Score = new MatchScoring();
    }

    public void EndMatch(Guid requestorId)
    {
        if (AdminId != requestorId)
            throw new InvalidOperationException("Only the admin can start the match");

        if (Status is MatchStatus.Finished or MatchStatus.Cancelled)
            throw new InvalidOperationException("Match has already ended");
        
        if(Status is MatchStatus.ReadyToStart)
            throw new InvalidOperationException("Cant end a match that has not started");
        
        Status = MatchStatus.Finished;
        EndTime = DateTime.UtcNow;
    }

    private void UpdateMatchTypeAndStatus()
    {
        if (TeamOne.Count == 1 && TeamTwo.Count == 1)
        {
            Type = MatchType.Single;
            Status = MatchStatus.ReadyToStart;
        }
        else if (TeamOne.Count == 2 && TeamTwo.Count == 2)
        {
            Type = MatchType.Double;
            Status = MatchStatus.ReadyToStart;
        }
        
        throw new InvalidOperationException("Invalid number of players");
    }
}