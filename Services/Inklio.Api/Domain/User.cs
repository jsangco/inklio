using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

public class User : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets the number of asks created by the user.
    /// </summary>
    public int AskCount { get; private set; }

    /// <summary>
    /// Gets the asks created by the user.
    /// </summary>
    public IReadOnlyCollection<Ask> Asks { get; private set; } = new List<Ask>();

    /// <summary>
    /// Gets the reputation earned by the user from asks.
    /// </summary>
    public int AskReputation { get; set; }

    /// <summary>
    /// Gets the ask tags created by the user.
    /// </summary>
    public IReadOnlyCollection<AskTag> AskTags { get; private set; } = new List<AskTag>();

    /// <summary>
    /// Gets the ask Upvotes created by the user.
    /// </summary>
    public IReadOnlyCollection<AskUpvote> AskUpvotes { get; private set; } = new List<AskUpvote>();

    /// <summary>
    /// Gets the comments created by the user.
    /// </summary>
    public IReadOnlyCollection<Comment> Comments { get; private set; } = new List<Comment>();

    /// <summary>
    /// Gets the comment Upvotes created by the user.
    /// </summary>
    public IReadOnlyCollection<CommentUpvote> CommentUpvotes { get; private set; } = new List<CommentUpvote>();

    /// <summary>
    /// Gets the UTC time the user was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the number of deliveries made by the user.
    /// </summary>
    public int DeliveryCount { get; private set; }

    /// <summary>
    /// Gets the deliveries made by the user.
    /// </summary>
    public IReadOnlyCollection<Delivery> Deliveries { get; private set; } = new List<Delivery>();

    /// <summary>
    /// Gets the Delivery Upvotes created by the user.
    /// </summary>
    public IReadOnlyCollection<DeliveryUpvote> DeliveryUpvotes { get; private set; } = new List<DeliveryUpvote>();

    /// <summary>
    /// Gets the deleviry tags created by the user.
    /// </summary>
    public IReadOnlyCollection<DeliveryTag> DeliveryTags { get; private set; } = new List<DeliveryTag>();

    /// <summary>
    /// Gets the reputation a user has earned by making deliveries. 
    /// </summary>
    public int DeliveryReputation { get; private set; }

    /// <summary>
    /// Gets the last time the user was active on the site.
    /// </summary>
    public DateTime LastActivityAtUtc { get; private set; }

    /// <summary>
    /// Gets the last time the user logged in to the site.
    /// </summary>
    public DateTime LastLoginAtUtc { get; private set; }

    /// <summary>
    /// Gets the reputation of the user including comments, asks, deliveries, etc...
    /// </summary>
    public int Reputation { get; private set; }

    /// <summary>
    /// Gets the UserId of the user.
    /// </summary>
    public UserId UserId { get; private set; }

    /// <summary>
    /// Gets the name of the user. 
    /// </summary>
    public string Username { get; private set; }

    /// <summary>
    /// Initializes a new <see cref="User"/> object. 
    /// </summary>
#pragma warning disable CS8618
    private User()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initializes a new <see cref="User"/> object. 
    /// </summary>
    /// <param name="username">The username</param>
    public User(UserId userId, string username)
    {
        this.Username = username;
        this.UserId = userId;
        this.DeliveryCount = 0;
        this.Reputation = 0;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.LastActivityAtUtc = DateTime.UtcNow;
        this.LastLoginAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Adjust the user's ask reputation and total reputation by a specified amount.
    /// </summary>
    /// <param name="adjustmentValue">The value to adjust by</param>
    public void AdjustDeliveryReputation(int adjustmentValue)
    {
        this.AskReputation += adjustmentValue;
        this.Reputation += adjustmentValue;
    }

    /// <summary>
    /// Adjust the user's reputation by a specified amount.
    /// </summary>
    /// <param name="adjustmentValue">The value to adjust by</param>
    public void AdjustAskReputation(int adjustmentValue)
    {
        this.Reputation += adjustmentValue;
        this.DeliveryReputation += adjustmentValue;
    }

    /// <summary>
    /// Sets the number of asks a user has made.
    /// </summary>
    /// <param name="askCount">The new ask count</param>
    public void SetAskCount(int askCount)
    {
        this.AskCount = askCount;
        this.LastActivityAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the number of deliveries a user has made.
    /// </summary>
    /// <param name="deliveryCount">The new delivery count</param>
    public void SetDeliveryCount(int deliveryCount)
    {
        this.DeliveryCount = deliveryCount;
        this.LastActivityAtUtc = DateTime.UtcNow;
    }
}