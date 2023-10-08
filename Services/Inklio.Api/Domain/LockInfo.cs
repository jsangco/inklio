using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// A deletion entity used to track post deletions
/// </summary>
public class LockInfo : Entity
{
    /// <summary>
    /// The id of the associated <see cref="Ask"/>.
    /// </summary>
    public int AskId { get; private set; }

    /// <summary>
    /// Gets the <see cref="Ask"/> associated with the lock.
    /// </summary>
    public Ask Ask { get; private set; }

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
    public LockType LockType { get; private set; }

    /// <summary>
    /// Gets the internal comment for the deletion
    /// </summary>
    public string InternalComment { get; private set; }

    /// <summary>
    /// Gets the message given to the user for the deletion.
    /// </summary>
    public string UserMessage { get; private set; }

#pragma warning disable CS8618
    /// <summary>
    /// Initalizes an instance of a <see cref="LockInfo"/> object.
    /// </summary>
    protected LockInfo()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initalizes an instance of a <see cref="LockInfo"/> object.
    /// </summary>
    /// <param name="ask">The associated ask.</param>
    /// <param name="createdBy">The user that created the deletion.</param>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public LockInfo(
        Ask ask,
        User createdBy,
        LockType deletionType,
        string internalComment,
        string userMessage)
    {
        this.Ask = ask;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.CreatedById = createdBy.Id;
        this.LockType = deletionType;
        this.InternalComment = internalComment;
        this.UserMessage = userMessage;
    }
}