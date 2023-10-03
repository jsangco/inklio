using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Represents a Delivery object
/// </summary>
public class Delivery : Entity, IAggregateRoot
{
    /// <summary>
    /// The maximum number of images that can be added to a Delivery.
    /// </summary>
    public const int MaxDeliveryImageCount = 5;

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
    /// Gets the content rating of the delivery.
    /// </summary>
    public byte ContentRating { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can comment on the delivery.
    /// </summary>
    public bool CanComment { get; private set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the delivery.
    /// </summary>
    public bool CanEdit { get; private set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the delivery.
    /// </summary>
    public bool CanFlag { get; private set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not can tag the delivery.
    /// </summary>
    public bool CanTag { get; private set; } = true;

    /// <summary>
    /// Gets or sets a collection of comments for the delivery.
    /// </summary>
    private List<DeliveryComment> comments = new List<DeliveryComment>();

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
    /// Gets username of the user that created the delivery.
    /// </summary>
    public string CreatedByUsername => this.CreatedBy.Username;

    /// <summary>
    /// Gets or sets the ID of the user that created the delivery.
    /// </summary>
    public int CreatedById { get; private set; }

    /// <summary>
    /// Gets the deletion associated with the delivery if it was deleted.
    /// </summary>
    public DeliveryDeletion? Deletion { get; private set; }

    /// <summary>
    /// The tags associated with the <see cref="Delivery"/>.
    /// </summary>
    private List<DeliveryTag> deliveryTags = new List<DeliveryTag>();

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
    /// The flags for the Delivery.
    /// </summary>
    private List<DeliveryFlag> flags = new List<DeliveryFlag>();

    /// <summary>
    /// Gets a collection of the <see cref="Delivery"/> object's flags.
    /// </summary>
    public IReadOnlyCollection<DeliveryFlag> Flags => this.flags;

    /// <summary>
    /// Gets or sets the number of times an account was flagged.
    /// </summary>
    public int FlagCount { get; private set; }

    /// <summary>
    /// The images associated with the ask.
    /// </summary>
    private List<DeliveryImage> images = new List<DeliveryImage>();

    /// <summary>
    /// Gets the images associated with the ask.
    /// </summary>m
    public IReadOnlyCollection<DeliveryImage> Images => this.images;

    /// <summary>
    /// Gets or sets a flag indicating whether the delivery is generated using AI
    /// </summary>
    public bool IsAi { get; private set; } = false;

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
    /// Gets a flag indicating whether or not the delivery contains a spoiler.
    /// </summary>
    public bool IsSpoiler { get; private set; } = false;

    /// <summary>
    /// Gets a flag indicating whether or not the delivery was upvoted by the user.
    /// This value is not stored in the database and is computed when a user
    /// retrieves the delivery.
    /// </summary>
    public bool IsUpvoted { get; private set; } = false;

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
        this.CreatedBy = new User(Guid.Empty, "");
    }

    /// <summary>
    /// Initializes an instance of a <see cref="Delivery"/> object.
    /// </summary>
    /// <param name="ask">The parent <see cref="Ask"/> object.</param>
    /// <param name="body">The body of the <see cref="Delivery"/></param>
    /// <param name="contentRating">The content rating of the <see cref="Delivery"/></param>
    /// <param name="createdBy">The creator of the <see cref="Delivery"/> object.</param>
    /// <param name="isAi">A flag indicating the <see cref="Delivery"/> is generated by AI.</param>
    /// <param name="isSpoiler">A flag indicating the <see cref="Delivery"/> contains a spoiler.</param>
    /// <param name="title">The title for the <see cref="Delivery"/> object.</param>
    public Delivery(Ask ask, string body, byte contentRating, User createdBy, bool isAi, bool isSpoiler, string title)
    {
        this.AskId = ask.Id;
        this.Ask = ask;
        this.Body = body;
        this.ContentRating = contentRating;
        this.CreatedBy = createdBy;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.IsAi = isAi;
        this.IsSpoiler = isSpoiler;
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
            throw new InklioDomainException(400, "Cannot unaccept the delivery because it has not been accepted yet.");
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
    /// Upvotes a comment of the delivery.
    /// </summary>
    /// <param name="commentId">The ide of the comment to upvote.</param>
    /// <param name="typeId">The type of the delivery</param>
    /// <param name="user">The user creating the upvote.</param>
    /// <returns>An <see cref="Upvote"/></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Upvote AddCommentUpvote(int commentId, int typeId, User user)
    {
        var comment = this.comments.FirstOrDefault(c => c.Id == commentId);
        if (comment is null)
        {
            throw new InklioDomainException(400, "Cannot add upvote to comment. The comment is not part of the delivery.");
        }
        var upvote = comment.AddUpvote(typeId, user);

        return upvote;
    }

    /// <summary>
    /// Adds a comment to the <see cref="Delivery"/> object.
    /// </summary>
    /// <param name="image">The image to add</param>
    /// <param name="createdBy">The user adding the blob</param>
    /// <returns>The newly created comment</returns>
    public void AddImage(DeliveryImage image, User createdBy)
    {
        this.ValidateCanAddImages(1, createdBy);
        this.images.Add(image);
    }

    /// <summary>
    /// Flags the <see cref="Delivery"/>.
    /// </summary>
    /// <param name="typeId">The type of the Flag.</param>
    /// <param name="user">The upvoting user.</param>
    public Flag AddFlag(FlagType typeId, User user)
    {
        int existingFlagIndex = this.flags.FindIndex(u => u.CreatedBy.Id == user.Id);
        if ( existingFlagIndex < 0)
        {
            var flag = new DeliveryFlag(this, (int)typeId, user);
            this.flags.Add(flag);
            this.FlagCount += 1;
            return flag;
        }

        return this.flags[existingFlagIndex];
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
        int existingUpvoteIndex = this.upvotes.FindIndex(u => u.CreatedById == user.Id);
        if (existingUpvoteIndex < 0)
        {
            var upvote = new DeliveryUpvote(this, typeId, user);
            this.upvotes.Add(upvote);
            this.UpvoteCount += 1;
            return upvote;
        }

        return this.upvotes[existingUpvoteIndex];
    }

    /// <summary>
    /// Marks an Delivery as deleted. It does not actually delete the Delivery.
    /// </summary>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="editor">The user deleting the Delivery.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="isModeratorDeletion">A flag indicating that this request was initiated by a moderator.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public void Delete(
        DeletionType deletionType,
        User editor,
        string internalComment,
        bool isModeratorDeletion,
        string userMessage)
    {
        if (this.IsDeleted)
        {
            return;
        }

        if (isModeratorDeletion == false && editor.Id != this.CreatedBy.Id)
        {
            throw new InklioDomainException(400, "User does not have permissions to delete this post.");
        }

        this.IsDeleted = true;
        this.EditedAtUtc = DateTime.UtcNow;
        this.EditedById = editor.Id;

        this.Deletion = new DeliveryDeletion(this, deletionType, editor, internalComment, userMessage);
    }

    /// <summary>
    /// Marks a comment on a delivery as deleted. It does not actually delete the comment.
    /// </summary>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="editor">The user deleting the comment.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="isModeratorDeletion">A flag indicating that this request was initiated by a moderator.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public void DeleteComment(
        int commentId,
        DeletionType deletionType,
        User editor,
        string internalComment,
        bool isModeratorDeletion,
        string userMessage)
    {
        var comment = this.comments.FirstOrDefault(d => d.Id == commentId);
        if (comment is null)
        {
            throw new InklioDomainException(400, "Cannot remove upvote from comment. The comment is not part of the Ask.");
        }
        comment.Delete(deletionType, editor, internalComment, isModeratorDeletion, userMessage);
    }

    /// <summary>
    /// Removes an upvote and removes the user from the list of upvoters.
    /// </summary>
    /// <param name="user">The user that created the upvote</param>
    public void DeleteUpvote(User user)
    {
        int upvoteIndex = this.upvotes.FindIndex(u => u.CreatedById == user.Id);
        if (upvoteIndex >= 0)
        {
            this.upvotes.RemoveAt(upvoteIndex);
            this.UpvoteCount -= 1;
        }
    }

    /// <summary>
    /// Deletes an upvote from a comment on the delivery.
    /// </summary>
    /// <param name="commentId">The id of the comment.</param>
    /// <param name="user">The user associated with the upvote.</param>
    public void DeleteCommentUpvote(int commentId, User user)
    {
        var comment = this.comments.FirstOrDefault(c => c.Id == commentId);
        if (comment is null)
        {
            throw new InklioDomainException(400, "Cannot remove upvote from comment. The comment is not part of the delivery.");
        }
        comment.DeleteUpvote(user);
    }

    /// <summary>
    /// Sets the IsUpvoted flag if the Upvotes list contains passed in user.
    /// </summary>
    /// <param name="user">The user who may have upvoted the post.</param>
    public void SetIsUpvoted(User user)
    {
        this.IsUpvoted = this.Upvotes.Any(u => u.CreatedById == user.Id);
    }

    /// <summary>
    /// Validates that a number of blobs can be added to a delivery.
    /// </summary>
    /// <param name="blobCount">The number of blobs to check that can be added.</param>
    /// <param name="createdby">The user that wants to add the blobs</param>
    /// <exception cref="InklioDomainException">An exception is thrown if the number of blobs cannot be added.</exception>
    public void ValidateCanAddImages(int blobCount, User createdBy)
    {
        if (this.CreatedBy != createdBy)
        {
            throw new InklioDomainException(403, $"Only the Delivery creator can add images.");
        }

        if (blobCount + this.images.Count > MaxDeliveryImageCount)
        {
            throw new InklioDomainException(400, $"Cannot add more than {MaxDeliveryImageCount} images to an Delivery.");
        }

        if (this.CanEdit == false)
        {
            throw new InklioDomainException(400, $"Unable to add images. This Delivery is no longer editable.");
        }
    }
}
