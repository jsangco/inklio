using Autofac;
using Inklio.Api.Domain;
using Inklio.Api.SeedWork;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Inklio.Api.Test;

public class ChallengeTests
{
    [Fact]
    public void Verify_CreateChallenge_will_start_immediately()
    {
        var user = new User(Guid.NewGuid(), "test");
        var ask = Ask.Create("my ask body", 1, user, false, 1, "my ask title");

        var utcNow = DateTime.UtcNow;
        var challenge = ask.AddChallenge(ChallengeType.Daily, utcNow, user, utcNow, utcNow + TimeSpan.FromMilliseconds(100));
        Assert.Equal(ChallengeState.Started, challenge.State);
        Assert.False(ask.IsLocked);
        Assert.Null(ask.LockInfo);
    }

    [Fact]
    public void Verify_CreateChallenge_will_end_immediately()
    {
        var user = new User(Guid.NewGuid(), "test");
        var ask = Ask.Create("my ask body", 1, user, false, 1, "my ask title");

        var utcNow = DateTime.UtcNow;
        var challenge = ask.AddChallenge(ChallengeType.Daily, utcNow, user, utcNow - TimeSpan.FromMilliseconds(200), utcNow - TimeSpan.FromMilliseconds(100));
        Assert.Equal(ChallengeState.Ended, challenge.State);
        Assert.True(ask.IsLocked);
        Assert.True(ask.LockInfo?.LockType == LockType.ChallengeEnded);
    }

    [Fact]
    public async Task Verify_CreateChallenge_starts_and_ends_correctly()
    {
        var user = new User(Guid.NewGuid(), "test");
        var ask = Ask.Create("my ask body", 1, user, false, 1, "my ask title");

        int delayMs = 100;
        var utcNow = DateTime.UtcNow;
        var startTime = utcNow + TimeSpan.FromMilliseconds(delayMs);
        var endTime = utcNow + TimeSpan.FromMilliseconds(delayMs * 2);
        var challenge = ask.AddChallenge(ChallengeType.Daily, utcNow, user, startTime, endTime);
        Assert.Equal(ChallengeState.NotStarted, challenge.State);
        Assert.True(ask.IsLocked);
        Assert.True(ask.LockInfo?.LockType == LockType.ChallengeNotStarted);
        await Task.Delay(delayMs);

        challenge.StartOrEndChallenge(user, startTime);
        Assert.Equal(ChallengeState.Started, challenge.State);
        Assert.False(ask.IsLocked);
        Assert.Null(ask.LockInfo);

        await Task.Delay(delayMs);

        challenge.StartOrEndChallenge(user, endTime);
        Assert.Equal(ChallengeState.Ended, challenge.State);
        Assert.True(ask.IsLocked);
        Assert.True(ask.LockInfo?.LockType == LockType.ChallengeEnded);
    }

    [Fact]
    public async Task Verify_ChallengeHostedService_starts_and_ends_challenges()
    {
        int delayMs = 800;
        var utcNow = DateTime.UtcNow;
        var user = new User(Guid.NewGuid(), "test");
        var neverStartsAsk = Ask.Create("my ask body", 1, user, false, 1, "my ask title"); neverStartsAsk.Id = 1;
        var startedAsk1 = Ask.Create("my ask body", 1, user, false, 1, "my ask title"); startedAsk1.Id = 2;
        var startedAsk2 = Ask.Create("my ask body", 1, user, false, 1, "my ask title"); startedAsk2.Id = 3;
        var endedAsk1 = Ask.Create("my ask body", 1, user, false, 1, "my ask title"); endedAsk1.Id = 4;
        var shouldHaveEndedAsk = Ask.Create("my ask body", 1, user, false, 1, "my ask title"); shouldHaveEndedAsk.Id = 5;

        Challenge neverStarts = neverStartsAsk.AddChallenge( // never starts
            ChallengeType.Daily,
            utcNow,
            user,
            utcNow + TimeSpan.FromMilliseconds(delayMs * 3),
            utcNow + TimeSpan.FromMilliseconds(delayMs * 4));
        Challenge started1 = startedAsk1.AddChallenge( // Starts after first delayMs
            ChallengeType.Daily,
            utcNow,
            user,
            utcNow + TimeSpan.FromMilliseconds(delayMs),
            utcNow + TimeSpan.FromMilliseconds(delayMs * 2));
        Challenge started2 = startedAsk2.AddChallenge( // Starts after second delayMs
            ChallengeType.Daily,
            utcNow,
            user,
            utcNow + TimeSpan.FromMilliseconds(delayMs * 2),
            utcNow + TimeSpan.FromMilliseconds(delayMs * 3));
        Challenge ends1 = endedAsk1.AddChallenge( // Begins started and ends after first delayMs
            ChallengeType.Daily,
            utcNow,
            user,
            utcNow + TimeSpan.FromMilliseconds(delayMs * -1),
            utcNow + TimeSpan.FromMilliseconds(delayMs));ends1.Id = 4;
        Challenge shouldHaveEnded = shouldHaveEndedAsk.AddChallenge( // Started in the past but was never ended
            ChallengeType.Daily,
            utcNow + TimeSpan.FromMilliseconds(delayMs * -2),
            user,
            utcNow + TimeSpan.FromMilliseconds(delayMs * -3),
            utcNow + TimeSpan.FromMilliseconds(delayMs * -1));shouldHaveEnded.Id = 5;
        shouldHaveEnded.StartOrEndChallenge(user, utcNow);

        IQueryable<Challenge> challenges = new Challenge[] { neverStarts, started1, started2, ends1, shouldHaveEnded }.AsQueryable<Challenge>();
        var askRepositoryMock = new Mock<IAskRepository>();
        askRepositoryMock.Setup(x => x.GetChallenges()).Returns(challenges);
        askRepositoryMock.Setup(x => x.UnitOfWork).Returns(new Mock<IUnitOfWork>().Object);
        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(x => x.GetOrAddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())) .ReturnsAsync(new User(Guid.NewGuid(), "aaa"));
        userRepositoryMock.Setup(x => x.UnitOfWork).Returns(new Mock<IUnitOfWork>().Object);
        ContainerBuilder builder = new ContainerBuilder();
        builder.Register(ctx => askRepositoryMock.Object).AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.Register(ctx => userRepositoryMock.Object).AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.Register(ctx => new NullLogger<ChallengeUpdater>()).AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<ChallengeUpdater>().AsSelf().InstancePerLifetimeScope();
        var container = builder.Build();
        var scope = container.BeginLifetimeScope();
        var hostedService = new ChallengeHostedService(
            new NullLogger<ChallengeHostedService>(),
            scope);

        Assert.Equal(ChallengeState.NotStarted, neverStarts.State);
        Assert.Equal(ChallengeState.NotStarted, started1.State);
        Assert.Equal(ChallengeState.NotStarted, started2.State);
        Assert.Equal(ChallengeState.Started, ends1.State);
        Assert.Equal(ChallengeState.Ended, shouldHaveEnded.State);
        await hostedService.StartAsync(default);

        await Task.Delay(1);
        Assert.Equal(ChallengeState.NotStarted, neverStarts.State);
        Assert.Equal(ChallengeState.NotStarted, started1.State);
        Assert.Equal(ChallengeState.NotStarted, started2.State);
        Assert.Equal(ChallengeState.Started, ends1.State);
        Assert.Equal(ChallengeState.Ended, shouldHaveEnded.State);

        await Task.Delay(delayMs);
        Assert.Equal(ChallengeState.NotStarted, neverStarts.State);
        Assert.Equal(ChallengeState.Started, started1.State);
        Assert.Equal(ChallengeState.NotStarted, started2.State);
        Assert.Equal(ChallengeState.Ended, ends1.State);
        Assert.Equal(ChallengeState.Ended, shouldHaveEnded.State);

        await Task.Delay(delayMs);
        Assert.Equal(ChallengeState.NotStarted, neverStarts.State);
        Assert.Equal(ChallengeState.Ended, started1.State);
        Assert.Equal(ChallengeState.Started, started2.State);
        Assert.Equal(ChallengeState.Ended, ends1.State);
        Assert.Equal(ChallengeState.Ended, shouldHaveEnded.State);

        await hostedService.StopAsync(default);
        Assert.Equal(ChallengeState.NotStarted, neverStarts.State);
        Assert.Equal(ChallengeState.Ended, started1.State);
        Assert.Equal(ChallengeState.Started, started2.State);
        Assert.Equal(ChallengeState.Ended, ends1.State);
        Assert.Equal(ChallengeState.Ended, shouldHaveEnded.State);
    }
}