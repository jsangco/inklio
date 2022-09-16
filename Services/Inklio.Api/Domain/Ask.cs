using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Domain reperesentation of an Ask
/// </summary>
public class Ask : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets the Body of the Ask.
    /// </summary>
    public string Body { get; private set; }

    /// <summary>
    /// Gets a flag indicating whether or not a user can comment on the Ask.
    /// </summary>
    public bool CanComment { get; private set; } = true;

    /// <summary>
    /// Gets a flag indicating whether or not deliveries can be added.
    /// </summary>
    public bool CanDeliver { get; private set; } = true;

    /// <summary>
    /// Gets a flag indicating whether or not a user can edit the ask.
    /// </summary>
    public bool CanEdit { get; private set; } = true;

    /// <summary>
    /// Gets a flag indicating whether or not a user can flag the ask.
    /// </summary>
    public bool CanFlag { get; private set; } = true;

    /// <summary>
    /// Gets a flag indicating whether or not can tag the ask.
    /// </summary>
    public bool CanTag { get; private set; } = true;

    /// <summary>
    /// The collection of comments for the <see cref="Ask"/>
    /// </summary>
    public List<AskComment> comments = new List<AskComment>();

    /// <summary>
    /// Gets the collection of comments for the <see cref="Ask"/>
    /// </summary>
    public IReadOnlyCollection<AskComment> Comments => this.comments;

    /// <summary>
    /// Gets the UTC time the ask was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the ID of the user that created the ask.
    /// </summary>
    public int CreatedById { get; private set; }

    /// <summary>
    /// The deliveries for the ask.
    /// </summary>
    private List<Delivery> deliveries = new List<Delivery>();

    /// <summary>
    /// Gets a collection of the <see cref="Ask"/> object's deliveries.
    /// </summary>
    public IReadOnlyCollection<Delivery> Deliveries => this.deliveries;

    /// <summary>
    /// Gets or sets the number of deliveries for the Ask. 
    /// </summary>
    public int DeliveryCount { get; set; }

    /// <summary>
    /// Gets or sets the number of accepted deliveries for the Ask.
    /// </summary>
    public int DeliveryAcceptedCount { get; set; }

    /// <summary>
    /// Gets the UTC time the ask was last edited.
    /// </summary>
    public DateTime? EditedAtUtc { get; private set; }

    /// <summary>
    /// Gets the id of the user that edited the ask.
    /// </summary>
    public int? EditedById { get; private set; }

    /// <summary>
    /// Gets or sets the number of times an account was flagged.
    /// </summary>
    public int FlagCount { get; set; }

    /// <summary>
    /// Gets a flag indicating whether or not the ask is deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Gets a flag indicating whether or not the ask has at least one delivery.
    /// </summary>
    public bool IsDelivered => DeliveryCount > 0;

    /// <summary>
    /// Gets a flag indicating whether or not the ask has an accepted delivery.
    /// </summary>
    public bool IsDeliveryAccepted => DeliveryAcceptedCount > 0;

    /// <summary>
    /// Gets a flag indicating whether or not the ask has been locked.
    /// </summary>
    public bool IsLocked { get; private set; }

    /// <summary>
    /// Gets a flag indicating whether or not the ask is NSFW.
    /// </summary>
    public bool IsNsfw { get; private set; } = false;

    /// <summary>
    /// Gets a flag indicating whether or not the ask NSFL.
    /// </summary>
    public bool IsNsfl { get; private set; } = false;

    /// <summary>
    /// Gets the UTC time that the ask was locked.
    /// </summary>
    public DateTime? LockedAtUtc { get; private set; }

    /// <summary>
    /// Gets or sets the number of times the ask was saved.
    /// </summary>
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets the Title of the Ask.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets or sets the number of times the ask was upvoted.
    /// </summary>
    public int UpvoteCount { get; set; }

    /// <summary>
    /// Gets or sets the number of times the ask has been viewed.
    /// </summary>
    public int ViewCount { get; set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="Ask"/> object.
    /// </summary>
    private Ask()
    {
        this.Body = string.Empty;
        this.Title = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of a <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Ask"/> object.</param>
    /// <param name="createdById">The ID of the creator of the <see cref="Ask"/> object.</param>
    /// <param name="isNsfl">A flag indicating whether the <see cref="Ask"/> is NSFL</param>
    /// <param name="isNsfw">A flag indicating whether the <see cref="Ask"/> is NSFW</param>
    /// <param name="title">The title of the <see cref="Ask"/> object.</param>
    public Ask(string body, int createdById, bool isNsfl, bool isNsfw, string title)
    {
        this.Body = body;
        this.CreatedById = createdById;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.IsNsfl = isNsfl;
        this.IsNsfw = isNsfw;
        this.Title = title;
    }

    /// <summary>
    /// Adds a comment to the <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body"></param>
    /// <param name="createdById"></param>
    /// <returns>The newly created comment</returns>
    public AskComment AddComment(string body, int createdById)
    {
        var comment = new AskComment(this, body, createdById);
        this.comments.Add(comment);
        return comment;
    }

    /// <summary>
    /// Adds a delivery to the <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Delivery"/>.</param>
    /// <param name="createdById">The ID of the user creating the <see cref="Delivery"/>.</param>
    /// <param name="isNsfl">A flag indicating whether the <see cref="Delivery"/> is NSFL.</param>
    /// <param name="isNsfw">A flag indicating whether the <see cref="Delivery"/> is NSFW.</param>
    /// <param name="title">The title of the Delivery.</param>
    /// <returns>The newly created <see cref="Delivery"/> object.</returns>
    public Delivery AddDelivery(string body, int createdById, bool isNsfl, bool isNsfw, string title)
    {
        var delivery = new Delivery(this, body, createdById, isNsfl, isNsfw, title);
        this.deliveries.Add(delivery);
        return delivery;
    }

    public void Delete() { }

    public void DeliveryAccept(int deliveryId) { }

    public void DeliveryAcceptUndo(int deliveryId) { }

    public void Edit(int userId, string edit) { }

    public void Flag(int userId) { }

    public void FlagUndo(int userId) { }

    public void Save(int userId) { }

    public void SaveUndo(int userId) { }

    public void Upvote(int userId) { }
    
    public void UpvoteUndo(int userId) { }
}