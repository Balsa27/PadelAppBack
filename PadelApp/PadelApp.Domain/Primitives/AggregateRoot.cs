namespace PadelApp.Domain.Primitives;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot() : base(){}
    
    private readonly List<IDomainEvent> _domainEvents = new();
    
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}