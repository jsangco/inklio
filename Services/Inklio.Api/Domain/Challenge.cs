using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Domain entity for a challenge.
/// </summary>
public class Challenge : Entity, IAggregateRoot
{
    /// <summary>
    /// The id of the associated <see cref="Ask"/>.
    /// </summary>
    public int AskId { get; private set; }

    /// <summary>
    /// The ask associated with the <see cref="Challenge"/>
    /// </summary>
    public Ask Ask { get; private set; }

    /// <summary>
    /// Gets the type of the <see cref="Challenge"/>.
    /// </summary>
    public ChallengeType ChallengeType { get; set; }
    
    /// <summary>
    /// Gets the UTC time the <see cref="Challenge"/> was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the user that created the challenge.
    /// </summary>
    public User CreatedBy { get; private set; }

    /// <summary>
    /// Gets or sets the ID of the user that created the challenge.
    /// </summary>
    public int CreatedById { get; private set; }

    /// <summary>
    /// Gets the UTC time the <see cref="Challenge"/> was ended.
    /// </summary>
    public DateTime EndAtUtc { get; private set; }

    /// <summary>
    /// Gets the UTC time the <see cref="Challenge"/> was started.
    /// </summary>
    public DateTime StartAtUtc { get; private set; }

    /// <summary>
    /// Gets a enum indicating the current state of the challenge
    /// </summary>
    public ChallengeState State { get; private set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="Challenge"/> object.
    /// </summary>
#pragma warning disable CS8618
    protected Challenge()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initializes a new instance of a <see cref="Challenge"/> object.
    /// </summary>
    /// <param name="ask">The ask associated with the challenge.</param>
    /// <param name="challengeType">The type of the <see cref="Challenge"/>.</param>
    /// <param name="createdAtUtc">The time the <see cref="Challenge"/> was created.</param>
    /// <param name="createdBy">The user that created the <see cref="Challenge"/>.</param>
    /// <param name="startAtUtc">The time the <see cref="Challenge"/> starts.</param>
    /// <param name="endAtUtc">The time the <see cref="Challenge"/> ends.</param>
    public Challenge(Ask ask, ChallengeType challengeType, DateTime createdAtUtc, User createdBy, DateTime startAtUtc, DateTime endAtUtc)
    {
        if (startAtUtc >= endAtUtc)
        {
            throw new ArgumentException("start time cannot be after the end time.");
        }

        this.Ask = ask;
        this.ChallengeType = challengeType;
        this.CreatedAtUtc = createdAtUtc;
        this.CreatedBy = createdBy;
        this.CreatedById = CreatedById;
        this.EndAtUtc = endAtUtc;
        this.StartAtUtc = startAtUtc;
        this.State = ChallengeState.NotStarted;
    }

    /// <summary>
    /// Starts or ends the challenge if it is within the activation time.
    /// </summary>
    /// <param name="user">The user triggering the start of the challenge.</param>
    /// <param name="utcNow">The current time.</param>
    public void StartOrEndChallenge(User user, DateTime utcNow)
    {
        if (utcNow < this.StartAtUtc) // Not started
        {
            this.Ask.Lock(user, LockType.ChallengeNotStarted, "", "The challenge has not started.");
            this.State = ChallengeState.NotStarted;
        }
        else if (utcNow >= this.StartAtUtc && utcNow < this.EndAtUtc) // Started
        {
            this.Ask.Unlock(user);
            this.State = ChallengeState.Started;
        }
        else if (utcNow >= this.EndAtUtc) // Ended
        {
            this.Ask.SetChallengeDeliveryRanks();
            this.Ask.Lock(user, LockType.ChallengeEnded, "", "The challenge has ended.");
            this.State = ChallengeState.Ended;
        }
        else // Something went wrong
        {
            throw new ArgumentOutOfRangeException(nameof(utcNow));
        }
    }
}