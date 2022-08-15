using Inklio.Api.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Inklio.Api.HealthCheck
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly InklioContext inklioContext;
        private readonly ILogger<DatabaseHealthCheck> logger;

        public DatabaseHealthCheck(InklioContext inklioContext, ILogger<DatabaseHealthCheck> logger)
        {
            this.inklioContext = inklioContext ?? throw new ArgumentNullException(nameof(inklioContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // Check if DB connection is successful to ensure that the service is ready.
            if (await this.inklioContext.Database.CanConnectAsync())
            {
                return HealthCheckResult.Healthy("Database connected.");
            }
            logger.LogWarning("Readiness Probe Failed");
            return HealthCheckResult.Unhealthy("Database connection failed.");
        }
    }
}
