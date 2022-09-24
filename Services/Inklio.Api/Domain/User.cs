using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

public class User : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets the asks created by the user.
    /// </summary>
    public IReadOnlyCollection<Ask> Asks { get; private set; }

    /// <summary>
    /// Gets the comments created by the user.
    /// </summary>
    public IReadOnlyCollection<Comment> Comments { get; private set; }

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
    public IReadOnlyCollection<Delivery> Deliveries { get; private set; }

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
    /// Gets the name of the user. 
    /// </summary>
    public string Username { get; private set; }

    /// <summary>
    /// Initializes a new <see cref="User"/> object. 
    /// </summary>
    private User()
    {
        this.Username = string.Empty;
        this.Asks = new List<Ask>();
        this.Comments = new List<Comment>();
        this.Deliveries = new List<Delivery>();
    }

    /// <summary>
    /// Initializes a new <see cref="User"/> object. 
    /// </summary>
    /// <param name="username">The username</param>
    public User(string username)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 32)
        {
            throw new ArgumentException($"'{nameof(username)}' must be at least 3 characters and less than 32 characters.", nameof(username));
        }

        this.Username = username;
        this.DeliveryCount = 0;
        this.Reputation = 0;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.LastActivityAtUtc = DateTime.UtcNow;
        this.LastLoginAtUtc = DateTime.UtcNow;
        this.Asks = new List<Ask>();
        this.Comments = new List<Comment>();
        this.Deliveries = new List<Delivery>();
    }

    /// <summary>
    /// Adjust the user's reputation by a specified amount.
    /// </summary>
    /// <param name="adjustmentValue">The value to adjust by</param>
    /// <param name="isDeliveryReputation">If the adjustment is related to a delivery.</param>
    public void AdjustReputation(int adjustmentValue, bool isDeliveryReputation)
    {
        this.Reputation += adjustmentValue;

        if (isDeliveryReputation)
        {
            this.DeliveryReputation += adjustmentValue;
        }
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