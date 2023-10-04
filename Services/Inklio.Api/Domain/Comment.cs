using Inklio.Api.SeedWork;
using Microsoft.OData.ModelBuilder;

namespace Inklio.Api.Domain;

/// <summary>
/// Reperesents a Comment
/// </summary>
public class Comment : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets or sets the Body of the comment.
    /// </summary>
    public string Body { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the comment.
    /// </summary>
    public bool CanEdit { get; private set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the comment.
    /// </summary>
    public bool CanFlag { get; private set; } = true;

    /// <summary>
    /// Gets or sets the UTC time the ask was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the user that created the comment.
    /// </summary>
    public User CreatedBy { get; private set; }

    /// <summary>
    /// Gets the username of the user that created the ask.
    /// </summary>
    public string CreatedByUsername => this.CreatedBy.Username;

    /// <summary>
    /// Gets the deletion associated with the comment if it was deleted.
    /// </summary>
    public CommentDeletion? Deletion { get; private set; }

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    public DateTime? EditedAtUtc { get; private set;}

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    public int? EditedById { get; private set; }

    /// <summary>
    /// The flags for the comment.
    /// </summary>
    private List<CommentFlag> flags = new List<CommentFlag>();

    /// <summary>
    /// Gets a collection of the <see cref="Comment"/> object's flags.
    /// </summary>
    public IReadOnlyCollection<CommentFlag> Flags => this.flags;

    /// <summary>
    /// Gets or sets the number of times an account was flagged.
    /// </summary>
    public int FlagCount { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the comment is deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the comment has been locked.
    /// </summary>
    public bool IsLocked { get; private set; }

    /// <summary>
    /// Gets a flag indicating whether or not the comment was upvoted by the user.
    /// This value is not stored in the database and is computed when a user
    /// retrieves the comment.
    /// </summary>
    public bool IsUpvoted { get; private set; }

    /// <summary>
    /// Gets or sets the UTC time that the comment was locked.
    /// </summary>
    public DateTime LockedAtUtc { get; private set; }

    /// <summary>
    /// Gets or sets the number of times the comment was saved.
    /// </summary>
    public int SaveCount { get; private set; }

    /// <summary>
    /// Gets the ID of the parent Ask
    /// </summary>
    public int ThreadId { get; private set; }

    /// <summary>
    /// Gets the parent Ask
    /// </summary>
    public Ask Thread { get; private set; }
    
    /// <summary>
    /// Gets or sets the number of times the comment was upvoted.
    /// </summary>
    public int UpvoteCount { get; set; }

    /// <summary>
    /// The list of upvotes for the comment
    /// </summary>
    private List<CommentUpvote> upvotes = new List<CommentUpvote>();

    /// <summary>
    /// Gets the list of upvotes for the comment
    /// </summary>
    public IReadOnlyCollection<CommentUpvote> Upvotes => this.upvotes;

    /// <summary>
    /// Initializes a new instance of a <see cref="Comment"/> object.
    /// </summary>
#pragma warning disable CS8618
    protected Comment()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="Comment"/>
    /// </summary>
    /// <param name="ask">The parent <see cref="Ask"/> object</param>
    /// <param name="body">The body of the comment</param>
    /// <param name="createdBy">The comment creator</param>
    protected Comment(Ask ask, string body, User createdBy)
    {
        this.Body = body;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.CreatedBy = createdBy;
        this.Thread = ask;
        this.ThreadId = ask.Id;
    }

    /// <summary>
    /// Flags the <see cref="Comment"/>.
    /// </summary>
    /// <param name="typeId">The type of the Flag.</param>
    /// <param name="user">The upvoting user.</param>
    public Flag AddFlag(FlagType typeId, User user)
    {
        int existingFlagIndex = this.flags.FindIndex(u => u.CreatedBy.Id == user.Id);
        if ( existingFlagIndex < 0)
        {
            var flag = new CommentFlag(this, (int)typeId, user);
            this.flags.Add(flag);
            this.FlagCount += 1;
            return flag;
        }

        return this.flags[existingFlagIndex];
    }

    /// <summary>
    /// Upvotes the <see cref="Comment"/>.
    /// </summary>
    /// <param name="typeId">The type of the upvote.</param>
    /// <param name="user">The upvoting user.</param>
    public Upvote AddUpvote(int typeId, User user)
    {
        int existingUpvoteIndex = this.upvotes.FindIndex(u => u.CreatedById == user.Id);
        if (existingUpvoteIndex < 0)
        {
            var upvote = new CommentUpvote(this, typeId, user);
            this.upvotes.Add(upvote);
            this.UpvoteCount += 1;
            this.CreatedBy.AdjustCommentReputation(1);
            return upvote;
        }

        return this.upvotes[existingUpvoteIndex];
    }

    /// <summary>
    /// Marks the comment as deleted. It does not actually delete the comment.
    /// </summary>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="editor">The user deleting the comment.</param>
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

        this.Deletion = new CommentDeletion(this, deletionType, editor, internalComment, userMessage);
    }

    /// <summary>
    /// Deletes an upvote and removes the user from the list of upvoters.
    /// </summary>
    /// <param name="user">The user associated with the upvote</param>
    public void DeleteUpvote(User user)
    {
        int upvoteIndex = this.upvotes.FindIndex(u => u.CreatedById == user.Id);
        if (upvoteIndex >= 0)
        {
            this.upvotes.RemoveAt(upvoteIndex);
            this.UpvoteCount -= 1;
            this.CreatedBy.AdjustCommentReputation(-1);
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