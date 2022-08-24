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
    /// Gets or sets the UTC time the comment was created.
    /// </summary>
    public DateTimeOffset CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user that created the comment.
    /// </summary>
    public int CreatedById { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    public DateTimeOffset EditedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the id of the user that edited the comment.
    /// </summary>    
    public int EditedById { get; set; }

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
    public DateTimeOffset LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the comment was saved.
    /// </summary>
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets the number of times the comment was upvoted.
    /// </summary>
    public int UpvoteCount { get; set; }
}