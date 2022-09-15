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
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the comment.
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the comment.
    /// </summary>
    public bool CanFlag { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the ask was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets the ID of the user that created the comment.
    /// </summary>
    public int CreatedById { get; set; }

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
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the comment has been locked.
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets the UTC time that the comment was locked.
    /// </summary>
    public DateTime LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the comment was saved.
    /// </summary>
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets the ID of the parent Ask
    /// </summary>
    // private readonly int threadId;

    /// <summary>
    /// Gets or sets the parent Ask
    /// </summary>
    public Ask Thread { get; set; } = new Ask();
    
    /// <summary>
    /// Gets or sets the number of times the comment was upvoted.
    /// </summary>
    public int UpvoteCount { get; set; }
}