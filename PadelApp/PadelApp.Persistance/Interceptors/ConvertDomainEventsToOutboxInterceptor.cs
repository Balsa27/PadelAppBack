using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using PadelApp.Domain.DomainEvents.DomainEventConverter;
using PadelApp.Domain.Primitives;
using PadelApp.Persistance.Outbox;

namespace PadelApp.Persistance.Interceptors;

public class ConvertDomainEventsToOutboxInterceptor 
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var outboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(x =>
            {
                var events = x.GetDomainEvents();
                x.ClearDomainEvents();
                return events;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }),
                OccurredOn = DateTime.UtcNow,
            })
            .ToList();
        
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}