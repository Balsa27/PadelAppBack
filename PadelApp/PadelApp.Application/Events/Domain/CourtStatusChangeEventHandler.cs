using MediatR;
using PadelApp.Application.Abstractions.Jobs;
using PadelApp.Domain.DomainEvents;
using Quartz;

namespace PadelApp.Application.Events.Domain;

public class CourtStatusChangeEventHandler : INotificationHandler<CourtStatusChangeDomainEvent>
{
    private readonly IScheduler _scheduler;
    private readonly IJobFactory _jobFactory;

    public CourtStatusChangeEventHandler(IScheduler scheduler, IJobFactory jobFactory)
    {
        _scheduler = scheduler;
        _jobFactory = jobFactory;
    }

    public async Task Handle(CourtStatusChangeDomainEvent notification, CancellationToken cancellationToken)
    {
        JobDataMap jobDataMap = new JobDataMap
        {
            { "CourtId", notification.CourtId.ToString() },
            { "Status", (int)notification.Status }
        };
        
        var job = _jobFactory.CreateCloseCourtJob(jobDataMap);
        
        ITrigger trigger = TriggerBuilder.Create()
            .StartAt(notification.LastOnGoingBookingEndTime)
            .Build();
        
        await _scheduler.ScheduleJob(job, trigger, cancellationToken);
    }
}