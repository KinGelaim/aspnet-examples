using Quartz;

namespace Scheduler.Quartz.Infrastructure.Quartz;

[DisallowConcurrentExecution]
public sealed class CreateInformationLogJob : IJob
{
    public const string Name = nameof(CreateInformationLogJob);

    private readonly ILogger<CreateInformationLogJob> _logger;
    private readonly TimeProvider _timeProvider;

    public CreateInformationLogJob(
        ILogger<CreateInformationLogJob> logger,
        TimeProvider timeProvider)
    {
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using var _ = _logger.BeginScope(
            "Job name: {JobName} StartTime: {StartTime}",
            nameof(CreateInformationLogJob),
            _timeProvider.GetUtcNow().UtcDateTime);

        _logger.LogDebug("Started");

        _logger.LogInformation($"Information log from the Job {_timeProvider.GetLocalNow()}");

        _logger.LogDebug("Finished");
    }
}