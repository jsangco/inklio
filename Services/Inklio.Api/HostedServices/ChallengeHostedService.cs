
using System.Runtime.CompilerServices;
using Inklio.Api.Domain;

public class ChallengeHostedService : IHostedService, IDisposable
{
    private readonly ILogger<ChallengeHostedService> logger;
    private readonly ILifetimeScope rootScope;
    private readonly IConfiguration? configuration;
    private Timer? timer;
    private List<Task> updateChallengeTasks = new List<Task>();

    public ChallengeHostedService(
        ILogger<ChallengeHostedService> logger,
        ILifetimeScope scope,
        IConfiguration? configuration = null)
    {
        this.logger = logger;
        this.rootScope = scope;
        this.configuration = configuration;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        var disabledHostedServices = this.configuration?.GetValue<string>("DisabledHostedServices") ?? "";
        if (disabledHostedServices.Contains(nameof(ChallengeHostedService)))
        {
            this.logger.LogInformation($"The {nameof(ChallengeHostedService)} is disabled.");
            return Task.CompletedTask;
        }

        this.logger.LogInformation($"Starting {nameof(ChallengeHostedService)}.");
        
        this.UpdateChallenges(null);

        return Task.CompletedTask;
    }

    private void UpdateChallenges(object? state)
    {
        this.updateChallengeTasks = this.updateChallengeTasks.Where(t => t.IsCompleted == false).ToList();
        this.updateChallengeTasks.Add(UpdateChallengesAsync(state));
    }

    private async Task UpdateChallengesAsync(object? state)
    {
        async Task<TimeSpan> DoUpdateAsync()
        {
            // This IHostedService runs forever in the background, so we want
            // to use a new lifetime scope every time the challenges are updated.
            // This ensures that things like the DbContext are properly disposed
            // and not maintained longer than they should be. More info:
            // https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#the-dbcontext-lifetime
            // https://docs.autofac.org/en/latest/lifetime/
            using var scope = this.rootScope.BeginLifetimeScope();
            var challengeUpdater = scope.Resolve<ChallengeUpdater>();
            return await challengeUpdater.UpdateChallengesAsync(null, CancellationToken.None);
        }

        TimeSpan dueTime = await DoUpdateAsync();
        this.timer?.Dispose();
        this.timer = new Timer(UpdateChallenges, null, dueTime, Timeout.InfiniteTimeSpan);
        this.logger.LogInformation($"Next challenge update due in: {dueTime.TotalSeconds}s ({DateTime.UtcNow + dueTime})");
    }

    public async Task StopAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("ChallengeHotedService is stopping.");

        this.timer?.Change(Timeout.Infinite, 0);
        await Task.WhenAll(this.updateChallengeTasks.ToArray());
    }

    public void Dispose()
    {
        this.timer?.Dispose();
    }
}