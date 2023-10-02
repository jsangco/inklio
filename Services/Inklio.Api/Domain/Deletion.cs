using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// A deletion entity used to track post deletions
/// </summary>
public class Deletion : Entity
{
    /// <summary>
    /// Gets the time the deletion was created
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// The ID of the <see cref="User"/> that created the upvote.
    /// </summary>
    public int CreatedById { get; private set; }

    /// <summary>
    /// Gets the type of the deletion
    /// </summary>
    public DeletionType DeletionType { get; }

    /// <summary>
    /// Gets the internal comment for the deletion
    /// </summary>
    public string InternalComment { get; }

    /// <summary>
    /// Gets the message given to the user for the deletion.
    /// </summary>
    public string UserMessage { get; }

#pragma warning disable CS8618
    /// <summary>
    /// Initalizes an instance of a <see cref="Deletion"/> object.
    /// </summary>
    protected Deletion()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initalizes an instance of a <see cref="Deletion"/> object.
    /// </summary>
    /// <param name="createdBy">The user that created the deletion.</param>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public Deletion(
        User createdBy,
        DeletionType deletionType,
        string internalComment,
        string userMessage)
    {
        this.CreatedAtUtc = DateTime.UtcNow;
        this.CreatedById = createdBy.Id;
        this.DeletionType = deletionType;
        this.InternalComment = internalComment;
        this.UserMessage = userMessage;
    }
}