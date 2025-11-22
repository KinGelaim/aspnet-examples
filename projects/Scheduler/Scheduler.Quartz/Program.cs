using CrystalQuartz.AspNetCore;
using Quartz;
using Scalar.AspNetCore;
using Scheduler.Quartz.Application;
using Scheduler.Quartz.Infrastructure.Quartz;
using Scheduler.Quartz.Infrastructure.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddQuartz();

builder.Services.GetOptionsAndBindOrThrow<FeatureToggleOptions>(
    builder.Configuration,
    FeatureToggleOptions.Position);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await QuartzJobsStartup.RunAsync(app.Services);

app.UseCrystalQuartz(() =>
    app.Services.GetRequiredService<ISchedulerFactory>().GetScheduler());

app.MapControllers();

app.Run();