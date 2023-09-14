using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Domain reperesentation of an Ask
/// </summary>
public class Ask : Entity, IAggregateRoot
{
    /// <summary>
    /// The maximum number of tags that can be added to an Ask
    /// </summary>
    public const int MaxTagCount = 15;

    /// <summary>
    /// The maximum number of images that can be added to an Ask.
    /// </summary>
    public const int MaxAskImageCount = 5;

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
    private List<AskComment> comments = new List<AskComment>();

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
    /// Gets or sets the ID of the user that created the delivery.
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
    public int DeliveryCount { get; private set; }

    /// <summary>
    /// Gets or sets the number of accepted deliveries for the Ask.
    /// </summary>
    public int DeliveryAcceptedCount { get; private set; }

    /// <summary>
    /// Gets the UTC time the ask was last edited.
    /// </summary>
    public DateTime? EditedAtUtc { get; private set; }

    /// <summary>
    /// Gets the id of the user that edited the ask.
    /// </summary>
    public int? EditedById { get; private set; }

    /// <summary>
    /// The flags for the ask.
    /// </summary>
    private List<AskFlag> flags = new List<AskFlag>();

    /// <summary>
    /// Gets a collection of the <see cref="Ask"/> object's flags.
    /// </summary>
    public IReadOnlyCollection<AskFlag> Flags => this.flags;

    /// <summary>
    /// Gets the number of times an account was flagged.
    /// </summary>
    public int FlagCount { get; private set; }

    /// <summary>
    /// The images associated with the ask.
    /// </summary>
    private List<AskImage> images = new List<AskImage>();

    /// <summary>
    /// Gets the images associated with the ask.
    /// </summary>m
    public IReadOnlyCollection<AskImage> Images => this.images;

    /// <summary>
    /// Gets a flag indicating whether or not the ask is deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Gets a flag indicating whether or not the ask has at least one delivery.
    /// </summary>
    public bool IsDelivered { get; private set; }

    /// <summary>
    /// Gets a flag indicating whether or not the ask has an accepted delivery.
    /// </summary>
    public bool IsDeliveryAccepted { get; private set; }

    /// <summary>
    /// Gets a flag indicating whether or not the ask has been locked.
    /// </summary>
    public bool IsLocked { get; private set; }

    /// <summary>
    /// Gets a flag indicating whether or not the ask NSFL.
    /// </summary>
    public bool IsNsfl { get; private set; } = false;

    /// <summary>
    /// Gets a flag indicating whether or not the ask is NSFW.
    /// </summary>
    public bool IsNsfw { get; private set; } = false;

    /// <summary>
    /// Gets a flag indicating whether or not the ask contains a spoiler.
    /// </summary>
    public bool IsSpoiler { get; private set; } = false;

    /// <summary>
    /// Gets the UTC time that the ask was locked.
    /// </summary>
    public DateTime? LockedAtUtc { get; private set; }

    /// <summary>
    /// Gets or sets the number of times the ask was saved.
    /// </summary>
    public int SaveCount { get; private set; }

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
    protected Ask()
    {
        this.Body = string.Empty;
        this.Title = string.Empty;
        this.CreatedBy = new User(Guid.Empty, "empty username");
    }

    /// <summary>
    /// Initializes a new instance of a <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Ask"/> object.</param>
    /// <param name="createdBy">The creator of the <see cref="Ask"/> object.</param>
    /// <param name="isNsfl">A flag indicating whether the <see cref="Ask"/> is NSFL</param>
    /// <param name="isNsfw">A flag indicating whether the <see cref="Ask"/> is NSFW</param>
    /// <param name="isSpoiler">A flag indicating whether the <see cref="Ask"/> is contains a spoiler</param>
    /// <param name="title">The title of the <see cref="Ask"/> object.</param>
    protected Ask(string body, User createdBy, bool isNsfl, bool isNsfw, bool isSpoiler, string title)
    {
        this.Body = body;
        this.CreatedBy = createdBy;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.IsNsfl = isNsfl;
        this.IsNsfw = isNsfw;
        this.IsSpoiler = isSpoiler;
        this.Title = title;
    }
    
    /// <summary>
    /// Creates a new instance of an <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Ask"/> object.</param>
    /// <param name="createdBy">The creator of the <see cref="Ask"/> object.</param>
    /// <param name="isNsfl">A flag indicating whether the <see cref="Ask"/> is NSFL.</param>
    /// <param name="isNsfw">A flag indicating whether the <see cref="Ask"/> is NSFW.</param>
    /// <param name="isNsfw">A flag indicating whether the <see cref="Ask"/> contains a spoiler.</param>
    /// <param name="title">The title of the <see cref="Ask"/> object.</param>
    public static Ask Create(string body, User createdBy, bool isNsfl, bool isNsfw, bool isSpoiler, string title)
    {
        // TODO - Future Validation
        // 1. User has not created any asks within the last X hours
        // 2. User has the ability to create a new ask
        // 3. Validate user can create the specified tag
        // 4. Validate the user can use the specified tag

        var ask = new Ask(body, createdBy, isNsfl, isNsfw, isSpoiler, title);
        return ask;
    }

    /// <summary>
    /// Mark a child delivery as accepted
    /// </summary>
    /// <param name="deliveryId">The delivery to accept</param>
    /// <exception cref="AskDomainException">An exception thrown when the delivery ID is not a child of the parent ask.</exception>
    public void AcceptDelivery(int deliveryId)
    {
        Delivery delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId) ?? throw new InklioDomainException(404, $"Could not accept delivery. Delivery id {deliveryId} was not found.");
        delivery.Accept();
        this.DeliveryAcceptedCount += 1;
        this.IsDeliveryAccepted = this.DeliveryAcceptedCount > 0;
    }

    /// <summary>
    /// Removes the accept status of a child delivery.
    /// </summary>
    /// <param name="deliveryId">The delivery to unaccept</param>
    /// <exception cref="AskDomainException">An exception thrown when the delivery ID is not a child of the parent ask.</exception>
    public void AcceptUndoDelivery(int deliveryId)
    {
        Delivery delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId) ?? throw new InklioDomainException(404, $"Could not undo the delivery accept. Delivery id {deliveryId} was not found.");
        delivery.AcceptUndo();
        this.DeliveryAcceptedCount -= 1;
        this.IsDeliveryAccepted = this.DeliveryAcceptedCount > 0;
    }

    /// <summary>
    /// Adds a comment to the <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body"></param>
    /// <param name="createdBy"></param>
    /// <returns>The newly created comment</returns>
    public AskComment AddComment(string body, User createdBy)
    {
        if (this.CanComment == false)
        {
            throw new InklioDomainException(400, "New comments cannot be added to this Ask.");
        }

        var comment = new AskComment(this, body, createdBy);
        this.comments.Add(comment);
        return comment;
    }

    /// <summary>
    /// Adds a delivery to the <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Delivery"/>.</param>
    /// <param name="createdBy">The user creating the <see cref="Delivery"/>.</param>
    /// <param name="isAi">A flag indicating whether the <see cref="Delivery"/> is generated by AI.</param>
    /// <param name="isNsfl">A flag indicating whether the <see cref="Delivery"/> is NSFL.</param>
    /// <param name="isNsfw">A flag indicating whether the <see cref="Delivery"/> is NSFW.</param>
    /// <param name="isNsfw">A flag indicating whether the <see cref="Delivery"/> contains a spoiler.</param>
    /// <param name="title">The title of the Delivery.</param>
    /// <returns>The newly created <see cref="Delivery"/> object.</returns>
    public Delivery AddDelivery(string body, User createdBy, bool isAi, bool isNsfl, bool isNsfw, bool isSpoiler, string title)
    {
        if (createdBy.Id == this.CreatedBy.Id)
        {
            throw new InklioDomainException(400, $"A delivery cannot be submitted by the original asker. User {createdBy.Username}");
        }

        var delivery = new Delivery(this, body, createdBy, isAi, isNsfl, isNsfw, isSpoiler, title);
        this.deliveries.Add(delivery);
        this.IsDelivered = this.deliveries.Count > 0;
        return delivery;
    }

    /// <summary>
    /// Flags the <see cref="Ask"/>.
    /// </summary>
    /// <param name="typeId">The type of the Flag.</param>
    /// <param name="user">The upvoting user.</param>
    public Flag AddFlag(FlagType typeId, User user)
    {
        int existingFlagIndex = this.flags.FindIndex(u => u.CreatedBy.Id == user.Id);
        if ( existingFlagIndex < 0)
        {
            var flag = new AskFlag(this, (int)typeId, user);
            this.flags.Add(flag);
            this.FlagCount += 1;
            return flag;
        }

        return this.flags[existingFlagIndex];
    }

    /// <summary>
    /// Adds a comment to the <see cref="Ask"/> object.
    /// </summary>
    /// <param name="image">The image to add</param>
    /// <param name="createdBy">The user adding the blob</param>
    /// <returns>The newly created comment</returns>
    public void AddImage(AskImage image, User createdBy)
    {
        this.ValidateCanAddImages(1, createdBy);
        this.images.Add(image);
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
        if (this.CanTag == false)
        {
            throw new InklioDomainException(400, "Tags cannot be added to this Ask.");
        }

        var existingTagIndex = this.askTags.FindIndex(t => t.TagId == tag.Id);
        if (existingTagIndex < 0)
        {
            if (this.askTags.Count > MaxTagCount)
            {
                throw new InklioDomainException(400, $"The number of tags added to the ask exceeded the maximum amount of {MaxTagCount}");
            }

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
    /// Validates that a number of blobs can be added to an ask.
    /// </summary>
    /// <param name="blobCount">The number of blobs to check that can be added.</param>
    /// <param name="createdby">The user that wants to add the blobs</param>
    /// <exception cref="InklioDomainException">An exception is thrown if the number of blobs cannot be added.</exception>
    public void ValidateCanAddImages(int blobCount, User createdBy)
    {
        if (this.CreatedBy != createdBy)
        {
            throw new InklioDomainException(403, $"Only the Ask creator can add images.");
        }

        if (blobCount + this.images.Count > MaxAskImageCount)
        {
            throw new InklioDomainException(400, $"Cannot add more than {MaxAskImageCount} images to an Ask.");
        }

        if (this.CanEdit == false)
        {
            throw new InklioDomainException(400, $"Unable to add images. This Ask is no longer editable.");
        }
    }

    /// <summary>
    /// Edits the <see cref="Ask"/> message body.
    /// </summary>
    /// <param name="bodyEdit">The new message body.</param>
    /// <param name="editor">The user making the edit.</param>
    public void EditBody(string bodyEdit, User editor)
    {
        this.Body = bodyEdit;
        this.EditedAtUtc = DateTime.UtcNow;
        this.EditedById = editor.Id;
    }

    /// <summary>
    /// Marks an Ask as deleted. NOTE: It does not actually delete the Ask.
    /// </summary>
    /// <param name="editor">The user deleting the Ask.</param>
    public void Delete(User editor)
    {
        this.IsDeleted = true;
        this.EditedAtUtc = DateTime.UtcNow;
        this.EditedById = editor.Id;
    }

    /// <summary>
    /// Removes a Tag from the ask.
    /// </summary>
    /// <param name="tag">The tag to remove</param>
    /// <param name="user">The user removing the tag.</param>
    public void RemoveTag(Tag tag, User user)
    {
        int tagIndex = this.tags.FindIndex(t => t.Id == tag.Id);
        if (tagIndex >= 0)
        {
            this.tags.RemoveAt(tagIndex);
        }
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