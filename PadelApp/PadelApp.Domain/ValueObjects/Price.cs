using PadelApp.Domain.Primitives;

namespace PadelApp.Domain.ValueObjects;

public class Price : Entity
{
    public decimal Amount { get; private set; }
    public TimeSpan Duration { get; private set; }
    public List<DayOfWeek> Days { get; private set; }
    public TimeSpan TimeStart { get; private set; }
    public TimeSpan TimeEnd { get; private set; }
    
    public Price(decimal amount, TimeSpan duration, TimeSpan startTime, TimeSpan endTime, List<DayOfWeek> daysOfWeek)
    {
        Amount = amount;
        Duration = duration;
        TimeStart = startTime;
        TimeEnd = endTime;
        Days = daysOfWeek;
    }
    
    public Price(Guid id, decimal amount, TimeSpan duration, TimeSpan startTime, TimeSpan endTime, List<DayOfWeek> daysOfWeek)
    {
        Id = id;    
        Amount = amount;
        Duration = duration;
        TimeStart = startTime;
        TimeEnd = endTime;
        Days = daysOfWeek;
    }

    public Price()
    {
        
    }

    public void Update(Price price)
    {
        Amount = price.Amount;
        Duration = price.Duration;
        TimeStart = price.TimeStart;
        TimeEnd = price.TimeEnd;
        Days = price.Days;
    }
}