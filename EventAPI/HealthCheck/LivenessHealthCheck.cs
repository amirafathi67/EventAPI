using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EventAPI.HealthCheck;

//Try not implement any logic(some sort of check) on “live” handler.
//https://dev.azure.com/kneatorg/git1601/_wiki/wikis/git1601.wiki/5278/Health-Checks
public class LivenessHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("Successfully initialized Event"));
    }
}