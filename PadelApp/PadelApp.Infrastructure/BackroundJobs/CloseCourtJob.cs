using PadelApp.Application.Abstractions.Repositories;
using PadelApp.Domain.Enums;
using Quartz;

namespace PadelApp.Infrastructure.BackroundJobs;

public class CloseCourtJob : IJob
{
    private readonly ICourtRepository _courtRepository;

    public CloseCourtJob(ICourtRepository courtRepository)
    {
        _courtRepository = courtRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        JobDataMap dataMap = context.JobDetail.JobDataMap;

        // Retrieve data passed to the job
        var courtId = dataMap.GetString("CourtId");
        
        if(string.IsNullOrEmpty(courtId))
            throw new ArgumentNullException(nameof(courtId));
        
        var status = (CourtStatus)dataMap.GetInt("Status");

        await _courtRepository.UpdateCourtStatus(Guid.Parse(courtId), status);
    }
}