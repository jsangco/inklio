using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Represents a Delivery object
/// </summary>
public class Delivery : Entity, IAggregateRoot
{

    /// <summary>
    /// Gets the UTC time the delivery was accepted at. 
    /// </summary>
    public DateTime? AcceptedAtUtc { get; private set; }

    /// <summary>
    /// Gets the UTC time the delivery accepted was undone at. 
    /// </summary>
    public DateTime? AcceptedUndoAtUtc { get; private set; }

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
    /// Gets the user that created the delivery.
    /// </summary>
    public User CreatedBy { get; private set; }

    /// <summary>
    /// The tags associated with the <see cref="Delivery"/>.
    /// </summary>
    private List<DeliveryTag> deliveryTags { get; set; } = new List<DeliveryTag>();

    /// <summary>
    /// Gets the tags associated with the <see cref="Delivery"/>.
    /// </summary>
    public IReadOnlyCollection<DeliveryTag> DeliveryTags => this.deliveryTags;

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
    /// The collection of tags assigned to the Delivery.
    /// </summary>
    private List<Tag> tags = new List<Tag>();

    /// <summary>
    /// Gets the collection of tags assigned to the Delivery.
    /// </summary>
    public IReadOnlyCollection<Tag> Tags => this.tags;

    /// <summary>
    /// Gets or sets the Title of the delivery.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times the delivery was upvoted.
    /// </summary>
    public int UpvoteCount { get; private set; }

    /// <summary>
    /// The list of upvotes for the
    /// </summary>
    private List<DeliveryUpvote> upvotes = new List<DeliveryUpvote>();

    /// <summary>
    /// Gets a list of all upvotes for the Delivery
    /// </summary>
    public IReadOnlyCollection<DeliveryUpvote> Upvotes => this.upvotes;

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
    /// <param name="createdBy">The creator of the <see cref="Delivery"/></param>
    /// <param name="isNsfl">A flag indicating the <see cref="Delivery"/> is NSFL</param>
    /// <param name="isNsfw">A flag indicating the <see cref="Delivery"/> is NSFW</param>
    /// <param name="title">The title for the <see cref="Delivery"/></param>
    public Delivery(Ask ask, string body, User createdBy, bool isNsfl, bool isNsfw, string title)
    {
        this.AskId = ask.Id;
        this.Ask = ask;
        this.Body = body;
        this.CreatedBy = createdBy;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.IsNsfl = isNsfl;
        this.IsNsfw = isNsfw;
        this.Title = title;
    }

    /// <summary>
    /// Marks a delivery as accepted by the parent Ask.
    /// </summary>
    public void Accept()
    {
        this.AcceptedAtUtc = DateTime.UtcNow;
        this.IsDeliveryAccepted = true;
    }

    /// <summary>
    /// Undos an accepted delivery.
    /// </summary>
    public void AcceptUndo()
    {
        if (this.IsDeliveryAccepted == false)
        {
            throw new AskDomainException("Cannot unaccept the delivery because it has not been accepted yet.");
        }

        this.AcceptedUndoAtUtc = DateTime.UtcNow;
        this.IsDeliveryAccepted = false;
    }

    /// <summary>
    /// Adds a comment to the delivery.
    /// </summary>
    /// <param name="body">The body of the <see cref="DeliveryComment"/>.</param>
    /// <param name="createdById">The creator of the <see cref="DeliveryComment"/>.</param>
    /// <returns>The newly created comment</returns>
    public DeliveryComment AddComment(string body, User createdBy)
    {
        var comment = new DeliveryComment(body, createdBy, this);
        this.comments.Add(comment);
        return comment;
    }

    /// <summary>
    /// Add a tag to the <see cref="Delivery"/> object.
    /// </summary>
    /// <param name="createdBy">The user who added the tag</param>
    /// <param name="tag">The tag to add.</param>
    public void AddTag(User createdBy, Tag tag)
    {
        var existingTagIndex = this.deliveryTags.FindIndex(t => t.TagId == tag.Id);
        if (existingTagIndex < 0)
        {
            this.deliveryTags.Add(new DeliveryTag(this, createdBy, tag));
            this.tags.Add(tag);
        }
    }
    
    /// <summary>
    /// Upvotes the <see cref="Delivery"/>.
    /// </summary>
    /// <param name="typeId">The type of the upvote.</param>
    /// <param name="user">The upvoting user.</param>
    public Upvote AddUpvote(int typeId, User user)
    {
        int existingUpvoteIndex = this.upvotes.FindIndex(u => u.createdById == user.Id);
        if ( existingUpvoteIndex < 0)
        {
            var upvote = new DeliveryUpvote(this, typeId, user);
            this.upvotes.Add(upvote);
            this.UpvoteCount += 1;
            return upvote;
        }

        return this.upvotes[existingUpvoteIndex];
    }

    /// <summary>
    /// Removes an upvote and removes the user from the list of upvoters.
    /// </summary>
    /// <param name="userId"></param>
    public void RemoveUpvote(Upvote upvote)
    {
        int upvoterIndex = this.upvotes.FindIndex(u => u.Id == upvote.Id);
        if ( upvoterIndex >= 0)
        {
            this.upvotes.RemoveAt(upvoterIndex);
            this.UpvoteCount -= 1;
        }
    }
}
