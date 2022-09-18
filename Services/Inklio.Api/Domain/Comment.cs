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
    /// Gets the ID of the user that created the comment.
    /// </summary>
    public int CreatedById { get; private set; }

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    public DateTime? EditedAtUtc { get; private set;}

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    public int? EditedById { get; private set; }

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
    public int SaveCount { get; set; }

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
    /// The list of users that have upvoted this comment.
    /// </summary>
    private List<User> upvoters = new List<User>();

    /// <summary>
    /// Gets a list of users that have upvoted the comment.
    /// </summary>
    public IReadOnlyCollection<User> Upvoters => this.upvoters;

    /// <summary>
    /// Initializes a new instance of a <see cref="Comment"/> object.
    /// </summary>
    private Comment()
    {
        this.Body = string.Empty;
        this.Thread = new Ask("unset body", 0, false, false, "unset title");
    }

    /// <summary>
    /// Initilaizes an instance of a <see cref="Comment"/>
    /// </summary>
    /// <param name="ask">The parent <see cref="Ask"/> object</param>
    /// <param name="body">The body of the comment</param>
    /// <param name="createdById">The id of the comment creator</param>
    public Comment(Ask ask, string body, int createdById)
    {
        this.Body = body;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.Thread = ask;
        this.ThreadId = ask.Id;
    }

    /// <summary>
    /// Increases the upvote count adds the user to the list of upvoters.
    /// </summary>
    /// <param name="user">The upvoting user.</param>
    public void Upvote(User user)
    {
        if (this.upvoters.FindIndex(u => u.Id == user.Id) < 0)
        {
            this.upvoters.Add(user);
            this.UpvoteCount += 1;
        }
    }
    
    /// <summary>
    /// Removes an upvote and removes the user from the list of upvoters.
    /// </summary>
    /// <param name="userId"></param>
    public void UpvoteUndo(User user)
    {
        int upvoterIndex = this.upvoters.FindIndex(u => u.Id == user.Id);
        if ( upvoterIndex >= 0)
        {
            this.upvoters.RemoveAt(upvoterIndex);
            this.UpvoteCount -= 1;
        }
    }
}