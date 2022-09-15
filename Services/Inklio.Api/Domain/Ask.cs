using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Domain reperesentation of an Ask
/// </summary>
public class Ask : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets or sets the Body of the Ask.
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can comment on the Ask.
    /// </summary>
    public bool CanComment { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not deliveries can be added.
    /// </summary>
    public bool CanDeliver { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the ask.
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the ask.
    /// </summary>
    public bool CanFlag { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not can tag the ask.
    /// </summary>
    public bool CanTag { get; set; }

    /// <summary>
    /// Gets or sets a collection of comments for the ask.
    /// </summary>
    public List<AskComment> Comments { get; private set; } = new List<AskComment>();

    /// <summary>
    /// Gets or sets the UTC time the ask was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the ID of the user that created the ask.
    /// </summary>
    public int CreatedById { get; private set; }

    /// <summary>
    /// Gets or sets the deliveries for the ask.
    /// </summary>
    public List<Delivery> Deliveries { get; private set; } = new List<Delivery>();

    /// <summary>
    /// Gets or sets the number of deliveries for the Ask. 
    /// </summary>
    public int DeliveryCount { get; set; }

    /// <summary>
    /// Gets or sets the number of accepted deliveries for the Ask.
    /// </summary>
    public int DeliveryAcceptedCount { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the ask was last edited.
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
    /// Gets or sets a flag indicating whether or not the ask is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has at least one delivery.
    /// </summary>
    public bool IsDelivered => DeliveryCount > 0;

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has an accepted delivery.
    /// </summary>
    public bool IsDeliveryAccepted => DeliveryAcceptedCount > 0;

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has been locked.
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask is NSFW.
    /// </summary>
    public bool IsNsfw { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask NSFL.
    /// </summary>
    public bool IsNsfl { get; private set; } = false;

    /// <summary>
    /// Gets or sets the UTC time that the ask was locked.
    /// </summary>
    public DateTime? LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the ask was saved.
    /// </summary>
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets the Title of the Ask.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times the ask was upvoted.
    /// </summary>
    public int UpvoteCount { get; set; }

    /// <summary>
    /// Gets or sets the number of times the ask has been viewed.
    /// </summary>
    public int ViewCount { get; set; }

    public Ask()
    {
        this.CreatedAtUtc = DateTime.UtcNow;
    }

    public void AddComment(AskComment comment)
    {
        this.EditedAtUtc = DateTime.UtcNow + TimeSpan.FromDays(1);
        comment.Thread = this;
        IsNsfl = true;
        this.Comments.Add(comment);
    }

    public void AddDelivery(Delivery delivery)
    {
        delivery.Ask = this;
        this.Deliveries.Add(delivery);
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