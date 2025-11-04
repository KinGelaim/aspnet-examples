using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck.Checks;

public class RandomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var rand = Random.Shared.Next(1, 4);
        var result = rand switch
        {
            1 => HealthCheckResult.Healthy("Healthy"),
            2 => HealthCheckResult.Degraded("Degraded"),
            _ => HealthCheckResult.Unhealthy("Unhealthy")
        };

        return Task.FromResult(result);
    }
}