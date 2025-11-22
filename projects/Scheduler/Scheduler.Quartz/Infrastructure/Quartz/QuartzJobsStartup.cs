using Quartz;

namespace Scheduler.Quartz.Infrastructure.Quartz;

public static class QuartzJobsStartup
{
    public static async Task RunAsync(IServiceProvider serviceProvider)
    {
        var factory = serviceProvider.GetRequiredService<ISchedulerFactory>();
        var scheduler = await factory.GetScheduler();

        await scheduler.Start();

        await ScheduleJobEveryNMinutes<CreateInformationLogJob>(
            scheduler,
            CreateInformationLogJob.Name,
            4);
    }

    private static async Task ScheduleJobEveryNMinutes<TJob>(
        IScheduler scheduler,
        string jobName,
        int minutes) where TJob : IJob
    {
        var job = JobBuilder.Create<TJob>()
            .WithIdentity(jobName)
            .Build();

        var jobTrigger = TriggerBuilder.Create()
            //.StartNow()
            /*.WithSimpleSchedule(x => x
                .WithIntervalInMinutes(minutes)
                .RepeatForever())*/
            .WithSchedule(
                CronScheduleBuilder
                    .CronSchedule($"0 0/{minutes} * * * ?")
                    .WithMisfireHandlingInstructionFireAndProceed())
            .Build();

        await scheduler.ScheduleJob(job, jobTrigger);
    }
}