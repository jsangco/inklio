using Inklio.Api.SeedWork;

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

    public static Comment Create(Ask ask, string body, User createdBy)
    {
        // TODO - Verify user can create a comment
        return new Comment(ask, body, createdBy);
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
        int existingUpvoteIndex = this.upvotes.FindIndex(u => u.CreatedBy.Id == user.Id);
        if ( existingUpvoteIndex < 0)
        {
            var upvote = new CommentUpvote(this, typeId, user);
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