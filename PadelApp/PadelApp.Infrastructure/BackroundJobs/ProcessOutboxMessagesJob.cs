using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PadelApp.Domain.DomainEvents.DomainEventConverter;
using PadelApp.Domain.Primitives;
using PadelApp.Persistance.EFC;
using PadelApp.Persistance.Outbox;
using Quartz;

namespace PadelApp.Infrastructure.BackroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(
        ApplicationDbContext dbContext,
        IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }
    
    //add try catch
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedDate == null)
            .Take(20)
            .ToListAsync();


        foreach (OutboxMessage message in messages)
        {
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(message.Content, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            
            // if(domainEvent is null)
            //     continue;

            await _publisher.Publish(domainEvent);
            
            message.ProcessedDate = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
    }
}