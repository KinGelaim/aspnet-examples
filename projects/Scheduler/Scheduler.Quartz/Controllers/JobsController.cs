using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Quartz;
using Scheduler.Quartz.Application;
using Scheduler.Quartz.Infrastructure.Quartz;

namespace Scheduler.Quartz.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class JobsController : ControllerBase
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<JobsController> _logger;
    private readonly FeatureToggleOptions _featureToggleOptions;

    public JobsController(
        ISchedulerFactory schedulerFactory,
        ILogger<JobsController> logger,
        IOptions<FeatureToggleOptions> featureToggleOptions)
    {
        _schedulerFactory = schedulerFactory;
        _logger = logger;
        _featureToggleOptions = featureToggleOptions.Value;
    }

    [HttpPost("run/create-information-log-job")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RunCreateInformationLogJob()
    {
        return await RunJobAsync(CreateInformationLogJob.Name);
    }

    private async Task<IActionResult> RunJobAsync(string jobName)
    {
        if (!_featureToggleOptions.ManualJobRunner)
        {
            return NotFound();
        }

        try
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var executingJobs = await scheduler.GetCurrentlyExecutingJobs();

            if (executingJobs.Any(x => x.JobDetail.Key.Name == jobName))
            {
                return BadRequest($"[{jobName}] запущена, пожалуйста подождите её завершения");
            }

            await scheduler.TriggerJob(new JobKey(jobName));

            return Ok($"[{jobName}] запущена");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occurred");

            return StatusCode(500);
        }
    }
}