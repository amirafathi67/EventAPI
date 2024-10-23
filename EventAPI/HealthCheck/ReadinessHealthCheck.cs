using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EventAPI.HealthCheck;

public class ReadinessHealthCheck : IHealthCheck
{
    private volatile bool _isReady;
    private readonly ILogger<ReadinessHealthCheck> _logger;
    private readonly Exception _exception;

    public bool StartupCompleted
    {
        get => _isReady;
        set => _isReady = value;
    }

    public ReadinessHealthCheck(IConfiguration config, ILogger<ReadinessHealthCheck> logger)
    {
        _logger = logger;
        StartupCompleted = true;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (StartupCompleted)
        {
            _logger.LogDebug("Health check: Successful.");
            return Task.FromResult(HealthCheckResult.Healthy());
        }

        _logger.LogError(_exception, "Health check: Unsuccessful.");
        return Task.FromResult(HealthCheckResult.Unhealthy());
    }
}