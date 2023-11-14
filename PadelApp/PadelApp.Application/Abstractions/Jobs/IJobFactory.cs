using Quartz;

namespace PadelApp.Application.Abstractions.Jobs;

public interface IJobFactory
{
    public IJobDetail CreateCloseCourtJob(JobDataMap jobDataMap);
}