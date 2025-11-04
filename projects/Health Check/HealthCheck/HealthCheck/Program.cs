using HealthCheck.Checks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services
    .AddHealthChecks()
    .AddCheck<AlwaysGoodHealthCheck>("AlwaysGood")
    .AddCheck<RandomHealthCheck>("Random", tags: ["random"]);

var app = builder.Build();

app.MapHealthChecks("AllChecks");

app.MapHealthChecks("/AllChecksWithMyResponseFormat", new HealthCheckOptions
{
    ResponseWriter = async (context, health) =>
    {
        if (health.Status == HealthStatus.Healthy)
        {
            await context.Response.WriteAsync("Everything is fine");
        }
        else
        {
            foreach (var h in health.Entries)
            {
                await context.Response.WriteAsync($"Key = {h.Key}, Description = {h.Value.Description}, Status = {h.Value.Status}\n");
            }

            await context.Response.WriteAsync($"\nOverall Status: {health.Status}");
        }
    }
});

app.MapHealthChecks("/OnlyRandomCheck", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("random")
});

app.MapDefaultControllerRoute();

app.Run();