using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Domain reperesentation of an Ask
/// </summary>
public class Ask : Entity, IAggregateRoot
{
    /// <summary>
    /// The tags associated with the ask.
    /// </summary>
    private List<AskTag> askTags { get; set; } = new List<AskTag>();

    /// <summary>
    /// Gets the tags associated with the ask.
    /// </summary>
    public IReadOnlyCollection<AskTag> AskTags => this.askTags;

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
    /// Gets the user that created the ask.
    /// </summary>
    public User CreatedBy { get; private set; }

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
    /// The collection of tags assigned to the ask.
    /// </summary>
    private List<Tag> tags = new List<Tag>();

    /// <summary>
    /// Gets the collection of tags assigned to the ask.
    /// </summary>
    public IReadOnlyCollection<Tag> Tags => this.tags;

    /// <summary>
    /// Gets the Title of the Ask.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets the number of times the ask was upvoted.
    /// </summary>
    public int UpvoteCount { get; private set; }

    /// <summary>
    /// The list of upvotes for the ask
    /// </summary>
    private List<AskUpvote> upvotes = new List<AskUpvote>();

    /// <summary>
    /// Gets the list of upvotes for the ask
    /// </summary>
    public IReadOnlyCollection<AskUpvote> Upvotes => this.upvotes;

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
        this.CreatedBy = new User("empty username");
    }

    /// <summary>
    /// Initializes a new instance of a <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Ask"/> object.</param>
    /// <param name="createdBy">The creator of the <see cref="Ask"/> object.</param>
    /// <param name="isNsfl">A flag indicating whether the <see cref="Ask"/> is NSFL</param>
    /// <param name="isNsfw">A flag indicating whether the <see cref="Ask"/> is NSFW</param>
    /// <param name="title">The title of the <see cref="Ask"/> object.</param>
    public Ask(string body, User createdBy, bool isNsfl, bool isNsfw, string title)
    {
        this.Body = body;
        this.CreatedBy = createdBy;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.IsNsfl = isNsfl;
        this.IsNsfw = isNsfw;
        this.Title = title;
    }

    /// <summary>
    /// Mark a child delivery as accepted
    /// </summary>
    /// <param name="deliveryId">The delivery to accept</param>
    /// <exception cref="AskDomainException">An exception thrown when the delivery ID is not a child of the parent ask.</exception>
    public void AcceptDelivery(int deliveryId)
    {
        Delivery delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId) ?? throw new AskDomainException($"Could not accept delivery. Delivery id {deliveryId} was not found.");
        delivery.Accept();
        this.DeliveryAcceptedCount += 1;
    }

    /// <summary>
    /// Removes the accept status of a child delivery.
    /// </summary>
    /// <param name="deliveryId">The delivery to unaccept</param>
    /// <exception cref="AskDomainException">An exception thrown when the delivery ID is not a child of the parent ask.</exception>
    public void AcceptUndoDelivery(int deliveryId)
    {
        Delivery delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId) ?? throw new AskDomainException($"Could not undo the delivery accept. Delivery id {deliveryId} was not found.");
        delivery.AcceptUndo();
        this.DeliveryAcceptedCount -= 1;
    }

    /// <summary>
    /// Adds a comment to the <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body"></param>
    /// <param name="createdBy"></param>
    /// <returns>The newly created comment</returns>
    public AskComment AddComment(string body, User createdBy)
    {
        var comment = new AskComment(this, body, createdBy);
        this.comments.Add(comment);
        return comment;
    }

    /// <summary>
    /// Adds a delivery to the <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Delivery"/>.</param>
    /// <param name="createdBy">The user creating the <see cref="Delivery"/>.</param>
    /// <param name="isNsfl">A flag indicating whether the <see cref="Delivery"/> is NSFL.</param>
    /// <param name="isNsfw">A flag indicating whether the <see cref="Delivery"/> is NSFW.</param>
    /// <param name="title">The title of the Delivery.</param>
    /// <returns>The newly created <see cref="Delivery"/> object.</returns>
    public Delivery AddDelivery(string body, User createdBy, bool isNsfl, bool isNsfw, string title)
    {
        if (createdBy.Id == this.CreatedBy.Id)
        {
            // TODO: Enable this exception once testing is over.
            // throw new AskDomainException($"A delivery cannot be submitted by the original asker. User Id {createdById}");
        }

        var delivery = new Delivery(this, body, createdBy, isNsfl, isNsfw, title);
        this.deliveries.Add(delivery);
        return delivery;
    }

    /// <summary>
    /// Add a tag to the <see cref="Ask"/> object.
    /// </summary>
    /// <param name="addToDeliveries">A flag indicating whether to add the tag to chilld deliveries</param>
    /// <param name="createdById">The id of the user who added the tag</param>
    /// <param name="Tag">The tag to add to the ask</param>
    /// <returns>The created tag</returns>
    public void AddTag(bool addToDeliveries, User createdBy, Tag tag)
    {
        var existingTagIndex = this.askTags.FindIndex(t => t.TagId == tag.Id);
        if (existingTagIndex < 0)
        {
            this.askTags.Add(new AskTag(this, createdBy, tag));
            this.tags.Add(tag);

            if (addToDeliveries)
            {
                foreach (var delivery in this.deliveries)
                {
                    delivery.AddTag(createdBy, tag);
                }
            }
        }
    }

    /// <summary>
    /// Upvotes the <see cref="Ask"/>.
    /// </summary>
    /// <param name="typeId">The type of the upvote.</param>
    /// <param name="user">The upvoting user.</param>
    public Upvote AddUpvote(int typeId, User user)
    {
        int existingUpvoteIndex = this.upvotes.FindIndex(u => u.CreatedBy.Id == user.Id);
        if ( existingUpvoteIndex < 0)
        {
            var upvote = new AskUpvote(this, typeId, user);
            this.upvotes.Add(upvote);
            this.UpvoteCount += 1;
            return upvote;
        }

        return this.upvotes[existingUpvoteIndex];
    }

    /// <summary>
    /// Edits the <see cref="Ask"/> message body.
    /// </summary>
    /// <param name="bodyEdit">The new message body.</param>
    /// <param name="userId">The Id of the user editting the body</param>
    public void EditBody(string bodyEdit, int userId)
    {
        this.Body = bodyEdit;
        this.EditedAtUtc = DateTime.UtcNow;
        this.EditedById = userId;
    }
    

    /// <summary>
    /// Marks an Ask as deleted. NOTE: It does not actually delete the Ask.
    /// </summary>
    /// <param name="userId">The Id of the user deleting Ask</param>
    public void Delete(int userId)
    {
        this.IsDeleted = true;
        this.EditedAtUtc = DateTime.UtcNow;
        this.EditedById = userId;
    }

    /// <summary>
    /// Removes an upvote and removes the user from the list of upvoters.
    /// </summary>
    /// <param name="userId"></param>
    public void RemoveUpvote(AskUpvote upvote)
    {
        int upvoterIndex = this.upvotes.FindIndex(u => u.Id == upvote.Id);
        if ( upvoterIndex >= 0)
        {
            this.upvotes.RemoveAt(upvoterIndex);
            this.UpvoteCount -= 1;
        }
    }
}