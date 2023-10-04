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
    /// Gets the number of comments on an ask.
    /// </summary>
    public int CommentCount { get; private set; }

    /// <summary>
    /// Gets the content rating of the ask.
    /// </summary>
    public byte ContentRating { get; set; }

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
    /// Gets or sets the ID of the user that created the ask.
    /// </summary>
    public int CreatedById { get; private set; }

    /// <summary>
    /// Gets the username of the user that created the ask.
    /// </summary>
    public string CreatedByUsername => this.CreatedBy.Username;

    /// <summary>
    /// Gets the deletion information for the ask if it was deleted.
    /// </summary>
    public AskDeletion? Deletion { get; set; }

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
    /// Gets a flag indicating whether or not the ask contains a spoiler.
    /// </summary>
    public bool IsSpoiler { get; private set; } = false;

    /// <summary>
    /// Gets a flag indicating whether or not the ask was upvoted by the user.
    /// This value is not stored in the database and is computed when a user
    /// retrieves the ask.
    /// </summary>
    public bool IsUpvoted { get; private set; } = false;

    /// <summary>
    /// Gets the UTC time that the ask was locked.
    /// </summary>
    public DateTime? LockedAtUtc { get; private set; }

    /// <summary>
    /// Gets the hot ranking of the ask which affects how the Ask is rendered on the front page.
    /// </summary>
    public int RankHot { get; private set; }

    /// <summary>
    /// Gets or the number of times the ask was saved.
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
        this.CreatedBy = new User(Guid.Empty, "");
    }

    /// <summary>
    /// Initializes a new instance of a <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Ask"/> object.</param>
    /// <param name="contentRating">The content rating of the <see cref="Ask"/> object.</param>
    /// <param name="createdBy">The creator of the <see cref="Ask"/> object.</param>
    /// <param name="isSpoiler">A flag indicating whether the <see cref="Ask"/> is contains a spoiler</param>
    /// <param name="rankHot">The hot ranking for the ask.</param>
    /// <param name="title">The title of the <see cref="Ask"/> object.</param>
    protected Ask(string body, byte contentRating, User createdBy, bool isSpoiler, int rankHot, string title)
    {
        this.Body = body;
        this.ContentRating = contentRating;
        this.CreatedBy = createdBy;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.IsSpoiler = isSpoiler;
        this.RankHot = rankHot;
        this.Title = title;
    }
    
    /// <summary>
    /// Creates a new instance of an <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Ask"/> object.</param>
    /// <param name="contentRating">The content rating of the <see cref="Ask"/> object.</param>
    /// <param name="createdBy">The creator of the <see cref="Ask"/> object.</param>
    /// <param name="isSpoiler">A flag indicating whether the <see cref="Ask"/> contains a spoiler.</param>
    /// <param name="rankHot">The hot ranking for the ask.</param>
    /// <param name="title">The title of the <see cref="Ask"/> object.</param>
    public static Ask Create(string body, byte contentRating, User createdBy, bool isSpoiler, int rankHot, string title)
    {
        // TODO - Future Validation
        // 1. User has not created any asks within the last X hours
        // 2. User has the ability to create a new ask
        // 3. Validate user can create the specified tag
        // 4. Validate the user can use the specified tag

        var ask = new Ask(body, contentRating, createdBy, isSpoiler, rankHot, title);
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
        this.CommentCount += 1;
        return comment;
    }

    /// <summary>
    /// Adds an upvote to a <see cref="Comment"/> on the <see cref="Ask"> object.
    /// </summary>
    /// <param name="commentId">The id of the comment to upvote</param>
    /// <param name="typeId">The type of the upvote.</param>
    /// <param name="createdById">The creator of the <see cref="DeliveryComment"/>.</param>
    /// <returns>The newly created comment</returns>
    public Upvote AddCommentUpvote(int commentId, int typeId, User createdBy)
    {
        var comment = this.Comments.FirstOrDefault(c => c.Id == commentId);
        if (comment is null)
        {
            throw new InklioDomainException(400, "Cannot add comment to comment. The comment is not part of the Ask.");
        }
        return comment.AddUpvote(typeId, createdBy);
    }

    /// <summary>
    /// Adds a delivery to the <see cref="Ask"/> object.
    /// </summary>
    /// <param name="body">The body of the <see cref="Delivery"/>.</param>
    /// <param name="contentRating">The content rating of the <see cref="Delivery"/>.</param>
    /// <param name="createdBy">The user creating the <see cref="Delivery"/>.</param>
    /// <param name="hottestRank">The current hottest rank of all <see cref="Ask"/>s.</param>
    /// <param name="isAi">A flag indicating whether the <see cref="Delivery"/> is generated by AI.</param>
    /// <param name="isSpoiler">A flag indicating whether the <see cref="Delivery"/> contains a spoiler.</param>
    /// <param name="title">The title of the Delivery.</param>
    /// <returns>The newly created <see cref="Delivery"/> object.</returns>
    public Delivery AddDelivery(string body, byte contentRating, User createdBy, int hottestRank, bool isAi, bool isSpoiler, string title)
    {
        if (createdBy.Id == this.CreatedBy.Id)
        {
            throw new InklioDomainException(400, $"A delivery cannot be submitted by the original asker. User: '{createdBy.Username}'");
        }

        var delivery = new Delivery(this, body, contentRating, createdBy, isAi, isSpoiler, title);
        this.deliveries.Add(delivery);
        this.IsDelivered = this.deliveries.Count > 0;
        this.DeliveryCount += 1;
        this.RankHot = hottestRank + 1;
        return delivery;
    }

    /// <summary>
    /// Adds a comment to a child delivery.
    /// </summary>
    /// <param name="body">The body of the <see cref="DeliveryComment"/>.</param>
    /// <param name="deliveryId">The id of the delivery.</param>
    /// <param name="createdById">The creator of the <see cref="DeliveryComment"/>.</param>
    /// <returns>The newly created comment</returns>
    public DeliveryComment AddDeliveryComment(string body, int deliveryId, User createdBy)
    {
        var delivery = this.Deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(400, "Cannot add comment to delivery. The delivery is not part of the Ask.");
        }
        this.CommentCount += 1;
        return delivery.AddComment(body, createdBy);
    }

    /// <summary>
    /// Upvotes a <see cref="Delivery"/> in the <see cref="Ask"/>.
    /// </summary>
    /// <param name="deliveryId">The id of the delivery.</param>
    /// <param name="typeId">The type of the upvote.</param>
    /// <param name="user">The upvoting user.</param>
    public bool AddDeliveryUpvote(int deliveryId, int typeId, User user)
    {
        var delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(400, "Cannot add upvote to delivery. The delivery is not part of the Ask.");
        }

        var wasUpvoted = delivery.AddUpvote(typeId, user);
        if (wasUpvoted)
        {
            this.RankHot += 1;
        }

        return wasUpvoted;
    }

    /// <summary>
    /// Upvotes a <see cref="DeliveryComment"/>'s in the <see cref="Ask"/>.
    /// </summary>
    /// <param name="commentId">The id of the comment.</param>
    /// <param name="deliveryId">The id of the delivery.</param>
    /// <param name="typeId">The type of the upvote.</param>
    /// <param name="user">The upvoting user.</param>
    public Upvote AddDeliveryCommentUpvote(int commentId, int deliveryId, int typeId, User user)
    {
        var delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(400, "Cannot add upvote to comment on delivery. The delivery is not part of the Ask.");
        }
        var upvote = delivery.AddCommentUpvote(commentId, typeId, user);

        return upvote;
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
        int existingUpvoteIndex = this.upvotes.FindIndex(u => u.CreatedById == user.Id);
        if ( existingUpvoteIndex < 0)
        {
            var upvote = new AskUpvote(this, typeId, user);
            this.upvotes.Add(upvote);
            this.UpvoteCount += 1;
            this.RankHot += 1;
            this.CreatedBy.AdjustAskReputation(1);
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
    /// Marks an Ask as deleted. It does not actually delete the Ask.
    /// </summary>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="editor">The user deleting the ask.</param>
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

        this.Deletion = new AskDeletion(this, deletionType, editor, internalComment, userMessage);
    }

    /// <summary>
    /// Marks a comment on an Ask as deleted. It does not actually delete the comment.
    /// </summary>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="editor">The user deleting the comment.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="isModeratorDeletion">A flag indicating that this request was initiated by a moderator.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public void DeleteAskComment(
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
            throw new InklioDomainException(400, "Cannot delete comment from ask. The comment is not part of the Ask.");
        }
        comment.Delete(deletionType, editor, internalComment, isModeratorDeletion, userMessage);
    }

    /// <summary>
    /// Removes an upvote from a comment.
    /// </summary>
    internal void DeleteAskCommentUpvote(int commentId, User user)
    {
        var comment = this.comments.FirstOrDefault(d => d.Id == commentId);
        if (comment is null)
        {
            throw new InklioDomainException(400, "Cannot remove upvote from comment. The comment is not part of the Ask.");
        }
        comment.DeleteUpvote(user);
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
            this.RankHot -= 1;
            this.CreatedBy.AdjustAskReputation(-1);
        }
    }

    /// <summary>
    /// Marks a delivery of an Ask as deleted. It does not actually delete the delivery.
    /// </summary>
    /// <param name="commentId">The id of the comment to delete</param>
    /// <param name="deliveryId">The id of the delivery containing the comment to delete.</param>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="editor">The user deleting the ask.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="isModeratorDeletion">A flag indicating that this request was initiated by a moderator.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public void DeleteDeliveryComment(
        int commentId,
        int deliveryId,
        DeletionType deletionType,
        User editor,
        string internalComment,
        bool isModeratorDeletion,
        string userMessage)
    {
        var delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(400, "Cannot remove upvote from delivery. The delivery is not part of the Ask.");
        }
        delivery.DeleteComment(commentId, deletionType, editor, internalComment, isModeratorDeletion, userMessage);
    }


    /// <summary>
    /// Marks a comment on an Ask as deleted. It does not actually delete the comment.
    /// </summary>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="editor">The user deleting the ask.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="isModeratorDeletion">A flag indicating that this request was initiated by a moderator.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public void DeleteDelivery(
        int deliveryId,
        DeletionType deletionType,
        User editor,
        string internalComment,
        bool isModeratorDeletion,
        string userMessage)
    {
        var delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(400, "Cannot remove upvote from delivery. The delivery is not part of the Ask.");
        }
        delivery.Delete(deletionType, editor, internalComment, isModeratorDeletion, userMessage);
    }

    /// <summary>
    /// Deletes an upvote from the delivery and removes the user from the list of upvoters.
    /// </summary>
    /// <param name="user">The user that created the upvote.</param>
    public bool DeleteDeliveryUpvote(int deliveryId, User user)
    {
        var delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(400, "Cannot remove upvote from delivery. The delivery is not part of the Ask.");
        }
        bool wasUpvoteDeleted = delivery.DeleteUpvote(user);
        if (wasUpvoteDeleted)
        {
            this.RankHot -= 1;
        }

        return wasUpvoteDeleted;
    }

    /// <summary>
    /// Deletes an upvote from a comment on a delivery.
    /// </summary>
    /// <param name="commentId">The ide of the comment.</param>
    /// <param name="deliveryId">The ide of the delivery.</param>
    /// <param name="user">The user upvote to delete.</param>
    /// <exception cref="InklioDomainException"></exception>
    public void DeleteDeliveryCommentUpvote(int commentId, int deliveryId, User user)
    {
        var delivery = this.deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(400, "Cannot remove upvote from comment on delivery. The delivery is not part of the Ask.");
        }
        delivery.DeleteCommentUpvote(commentId, user);
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
    /// Sets the IsUpvoted flag if the Upvotes list contains passed in user.
    /// </summary>
    /// <param name="user">The user who may have upvoted the post.</param>
    public void SetIsUpvoted(User user)
    {
        this.IsUpvoted = this.Upvotes.Any(u => u.CreatedById == user.Id);
    }
}
