using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Scheduler.Hangfire.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class JobsController : ControllerBase
{
    [HttpGet("run-fire-and-forget-job")]
    public IActionResult RunFireAndForgetJob()
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget job executed!"));
        return Ok("Fire-and-forget job enqueued.");
    }

    [HttpGet("run-delayed-job")]
    public IActionResult RunDelayedJob()
    {
        BackgroundJob.Schedule(() => Console.WriteLine("Delayed job executed after 10 seconds!"), TimeSpan.FromSeconds(10));
        return Ok("Delayed job enqueued.");
    }

    [HttpGet("run-recurring-job")]
    public IActionResult RunRecurringJob()
    {
        RecurringJob.AddOrUpdate("my-recurring-job", () => Console.WriteLine("Recurring job executed!"), Cron.Minutely);
        return Ok("Recurring job enqueued (runs every minute).");
    }

    [HttpGet("run-di-job")]
    public IActionResult RunDiJob(IBackgroundJobClient jobClient)
    {
        jobClient.Schedule<ExampleJob>(x => x.DoSomething(), TimeSpan.FromSeconds(10));
        return Ok("Delayed job enqueued (from DI).");
    }
}