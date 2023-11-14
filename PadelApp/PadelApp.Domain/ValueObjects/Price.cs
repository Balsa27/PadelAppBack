using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.ValueObjects;

public class Price : ValueObject
{
    public decimal Amount { get; private set; }
    public TimeSpan Duration { get; private set; }
    public DayOfWeek? Day { get; private set; }
    public TimeSpan? TimeStart { get; private set; }
    public TimeSpan? TimeEnd { get; private set; }
    
    public Price(
        decimal amount,
        TimeSpan duration,
        DayOfWeek? day = null,
        TimeSpan? timeStart = null,
        TimeSpan? timeEnd = null)
    {   
        Amount = amount;
        Duration = duration;
        Day = day;
        TimeStart = timeStart;
        TimeEnd = timeEnd;
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Amount;
        yield return Duration;
        yield return Day;
        yield return TimeStart;
        yield return TimeEnd;
    }
}