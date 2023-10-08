
using Inklio.Api.Domain;

public class ChallengeUpdater
{
    private readonly ILogger<ChallengeUpdater> logger;
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;
    private User challengeMod = new User(new Guid("ba8f9975-a773-4c19-ab3a-f10423ac692e"), "ChallengeMod");

    /// <summary>
    /// Initiaizes a new instance of a <see cref="ChallengeUpdater"/> object.
    /// </summary>
    /// <param name="logger">A logger.</param>
    /// <param name="askRepository">An ask repository.</param>
    /// <param name="userRepository">A user repository.</param>
    public ChallengeUpdater(
        ILogger<ChallengeUpdater> logger,
        IAskRepository askRepository,
        IUserRepository userRepository)
    {
        this.logger = logger;
        this.askRepository = askRepository;
        this.userRepository = userRepository;
    }

    /// <summary>
    /// Checks challenges to see if one needs to be started or properly ended. Then returns
    /// the length of time until the next challenge should be updated.
    /// </summary>
    /// <param name="state">An optional state param.</parm>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The length of time until the next challenge should be updated.</returns>
    public async Task<TimeSpan> UpdateChallengesAsync(object? state, CancellationToken cancellationToken)
    {
        try
        {
            return await UpdateChallengesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            // Ensure that the timer continues if an exception occurs.
            var dueTime = TimeSpan.FromMinutes(1);
            this.logger.LogError(e.ToString());
            return dueTime;
        }
    }

    /// <summary>
    /// Checks challenges to see if one needs to be started or properly ended. Then returns
    /// the length of time until the next challenge should be updated.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The length of time until the next challenge should be updated.</returns>
    private async Task<TimeSpan> UpdateChallengesAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Updating challenges");

        // Get moderator user for challenges.
        var user = await this.userRepository.GetOrAddUserAsync(challengeMod, default);
        await this.userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        // Get challenges challenges that have not ended and start or end them.
        var utcNow = DateTime.UtcNow;
        var challenges = this.askRepository.GetChallenges()
            .Include(e => e.Ask)
            .Include(e => e.Ask).ThenInclude(e => e.Deliveries)
            .Where(c => c.State == ChallengeState.NotStarted || c.State == ChallengeState.Started);
        foreach (var challenge in challenges)
        {
            challenge.StartOrEndChallenge(user, utcNow);
        }
        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        // Set timer to trigger at the start or end of the next challenge
        var nextStartChallenge = this.askRepository.GetChallenges()
            .Where(c => c.State == ChallengeState.NotStarted)
            .OrderBy(c => c.StartAtUtc)
            .FirstOrDefault();
        var nextEndChallenge = this.askRepository.GetChallenges()
            .Where(c => c.State == ChallengeState.Started)
            .OrderBy(c => c.EndAtUtc)
            .FirstOrDefault();

        // If there happens to be no challenges, use null coalescing and terinary magic to ensure we have some sort of next trigger time.
        DateTime nextStartTime = nextStartChallenge?.StartAtUtc ?? nextEndChallenge?.EndAtUtc ?? utcNow + TimeSpan.FromHours(1);
        DateTime nextEndTime = nextEndChallenge?.EndAtUtc ?? nextStartChallenge?.StartAtUtc ?? utcNow + TimeSpan.FromHours(1);
        DateTime timeOfNextStart = nextStartTime < nextEndTime ? nextStartTime : nextEndTime;
        TimeSpan timeUntilNextStartOrEnd = timeOfNextStart - utcNow;
        TimeSpan dueTime = // Clamp dueTime to 0 and 1 hours.
            timeUntilNextStartOrEnd.TotalMilliseconds > 0 ?
                timeUntilNextStartOrEnd.TotalHours < 1 ?
                    timeUntilNextStartOrEnd :
                    TimeSpan.FromHours(1) :
                TimeSpan.Zero;

        return dueTime;
    }
}
