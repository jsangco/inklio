using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Represents a Delivery object
/// </summary>
public class Delivery : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets the ID for the parent <see cref="Ask"/>
    /// </summary>
    public int AskId { get; private set; }

    /// <summary>
    /// Gets the parent <see cref="Ask"/> that owns the <see cref="Delivery"/>
    /// </summary>
    public Ask Ask { get; private set; }

    /// <summary>
    /// Gets or sets the Body of the delivery.
    /// </summary>
    public string Body { get; private set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can comment on the delivery.
    /// </summary>
    public bool CanComment { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not deliveries can be added.
    /// </summary>
    public bool CanDeliver { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the delivery.
    /// </summary>
    public bool CanEdit { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the delivery.
    /// </summary>
    public bool CanFlag { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not can tag the delivery.
    /// </summary>
    public bool CanTag { get; private set; }

    /// <summary>
    /// Gets or sets a collection of comments for the delivery.
    /// </summary>
    public List<DeliveryComment> comments = new List<DeliveryComment>();

    /// <summary>
    /// A collection of comments for the delivery.
    /// </summary>
    public IReadOnlyCollection<DeliveryComment> Comments => this.comments;

    /// <summary>
    /// Gets the UTC time the delivery was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the ID of the user that created the delivery.
    /// </summary>
    public int CreatedById { get; private set; }

    /// <summary>
    /// Gets or sets the UTC time the delivery was last edited.
    /// </summary>
    public DateTime? EditedAtUtc { get; private set; }

    /// <summary>
    /// Gets or sets the UTC time the delivery was last edited.
    /// </summary>
    public int? EditedById { get; private set; }

    /// <summary>
    /// Gets or sets the number of times an account was flagged.
    /// </summary>
    public int FlagCount { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery is deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery has an accepted delivery.
    /// </summary>
    public bool IsDeliveryAccepted { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery has been locked.
    /// </summary>
    public bool IsLocked { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery is NSFW.
    /// </summary>
    public bool IsNsfw { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery NSFL.
    /// </summary>
    public bool IsNsfl { get; private set; }

    /// <summary>
    /// Gets or sets the UTC time that the delivery was locked.
    /// </summary>
    public DateTime? LockedAtUtc { get; private set; } = null;

    /// <summary>
    /// Gets or sets the number of times the delivery was saved.
    /// </summary>
    public int SaveCount { get; private set; }

    /// <summary>
    /// Gets or sets the Title of the delivery.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times the delivery was upvoted.
    /// </summary>
    public int UpvoteCount { get; private set; }

    /// <summary>
    /// Gets or sets the number of times the delivery has been viewed.
    /// </summary>
    public int ViewCount { get; private set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="Delivery"/> object.
    /// </summary>
#pragma warning disable CS8618
    private Delivery()
#pragma warning restore CS8618
    {
    }

    /// <summary>
    /// Initializes an instance of a <see cref="Delivery"/> object.
    /// </summary>
    /// <param name="ask">The parent <see cref="Ask"/> object.</param>
    /// <param name="body">The body of the <see cref="Delivery"/></param>
    /// <param name="createdById">The creator of the <see cref="Delivery"/></param>
    /// <param name="isNsfl">A flag indicating the <see cref="Delivery"/> is NSFL</param>
    /// <param name="isNsfw">A flag indicating the <see cref="Delivery"/> is NSFW</param>
    /// <param name="title">The title for the <see cref="Delivery"/></param>
    public Delivery(Ask ask, string body, int createdById, bool isNsfl, bool isNsfw, string title)
    {
        this.AskId = ask.Id;
        this.Ask = ask;
        this.Body = body;
        this.CreatedById = createdById;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.IsNsfl = isNsfl;
        this.IsNsfw = isNsfw;
        this.Title = title;
    }

    /// <summary>
    /// Adds a comment to the delivery.
    /// </summary>
    /// <param name="body">The body of the <see cref="DeliveryComment"/>.</param>
    /// <param name="createdById">The creator of the <see cref="DeliveryComment"/>.</param>
    /// <returns>The newly created comment</returns>
    public DeliveryComment AddComment(string body, int createdById)
    {
        var comment = new DeliveryComment(body, createdById, this);
        this.comments.Add(comment);
        return comment;
    }
}
