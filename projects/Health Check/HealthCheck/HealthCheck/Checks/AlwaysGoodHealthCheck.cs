using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck.Checks;

public sealed class AlwaysGoodHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}