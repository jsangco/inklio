using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Inklio.Auth.HealthCheck;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IdentityDataContext context;
    private readonly ILogger<DatabaseHealthCheck> logger;

    public DatabaseHealthCheck(IdentityDataContext context, ILogger<DatabaseHealthCheck> logger)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // Check if DB connection is successful to ensure that the service is ready.
        if (await this.context.Database.CanConnectAsync())
        {
            return HealthCheckResult.Healthy("Database connected.");
        }
        logger.LogWarning("Readiness Probe Failed");
        return HealthCheckResult.Unhealthy("Database connection failed.");
    }
}
