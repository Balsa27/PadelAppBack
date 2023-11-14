using PadelApp.Application.Abstractions.Jobs;
using Quartz;

namespace PadelApp.Infrastructure.BackroundJobs;

public class QuartzJobFactory : IJobFactory
{
    public IJobDetail CreateCloseCourtJob(JobDataMap jobDataMap)
    {
        return JobBuilder.Create<CloseCourtJob>()
            .SetJobData(jobDataMap)
            .Build();
    }
}