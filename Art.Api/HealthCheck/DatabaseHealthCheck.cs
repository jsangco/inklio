using Art.Api.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Art.Api.HealthCheck
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ArtContext artContext;
        private readonly ILogger<DatabaseHealthCheck> logger;

        public DatabaseHealthCheck(ILogger<DatabaseHealthCheck> logger)
        {
            //this.artContext = artContext ?? throw new ArgumentNullException(nameof(artContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //// Check if DB connection is successful to ensure that the service is ready.
            //if (await artContext.Database.CanConnectAsync())
            //{
                //return HealthCheckResult.Healthy("Database connection is successful.");
            //}
            //logger.LogWarning("Readiness Probe Failed");
            //return HealthCheckResult.Unhealthy("Database connection failed.");
            return HealthCheckResult.Healthy("ready");
        }
    }
}
