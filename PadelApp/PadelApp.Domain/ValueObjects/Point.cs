using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.ValueObjects;

public class Point : ValueObject
{
    public Guid ScorerId { get; private set; }
    public DateTime TimeScored { get; private set; }

    public Point(Guid scorerId, DateTime timeScored)
    {
        ScorerId = scorerId;
        TimeScored = timeScored;
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return ScorerId;
        yield return TimeScored;
    }
}