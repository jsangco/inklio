using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Domain for a challenge rank.
/// </summary>
public class ChallengeDeliveryRank : Entity, IAggregateRoot
{
    /// <summary>
    /// The id of the associated <see cref="Ask"/>.
    /// </summary>
    public int AskId { get; private set; }

    /// <summary>
    /// Gets the associated <see cref="Ask"/>.
    /// </summary>
    public Ask Ask { get; private set; }

    /// <summary>
    /// The id of the associated <see cref="Challenge"/>.
    /// </summary>
    public int ChallengeId { get; private set; }

    /// <summary>
    /// Gets the associated <see cref="Challenge"/>
    /// </summary>
    public Challenge Challenge { get; private set; }

    /// <summary>
    /// Gets the UTC time the <see cref="ChallengeDeliveryRank"/> was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// The id of the associated <see cref="Delivery"/>.
    /// </summary>
    public int DeliveryId { get; private set; }

    /// <summary>
    /// The <see cref="Delivery"/> associated with the rank
    /// </summary>
    public Delivery Delivery { get; private set; }

    /// <summary>
    /// Gets the rank.
    /// </summary>
    public int Rank { get; private set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="ChallengeDeliveryRank"/> object.
    /// </summary>
#pragma warning disable CS8618
    protected ChallengeDeliveryRank()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initializes a new instance of a <see cref="ChallengeDeliveryRank"/> object.
    /// </summary>
    /// <param name="ask">The associted <see cref="Ask"/>.</param>
    /// <param name="challenge">The associated <see cref="Challenge"/>.</param>
    /// <param name="delivery">The associted <see cref="Delivery"/>.</param>
    /// <param name="rank">The rank of the <see cref="Delivery"/>.</param>
    public ChallengeDeliveryRank(Ask ask, Challenge challenge, Delivery delivery, int rank)
    {
        this.Ask = ask;
        this.Challenge = challenge;
        this.Delivery = delivery;
        this.Rank = rank;
    }
}