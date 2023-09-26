using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An upvote entity used to track user upvotes.
/// </summary>
public class Upvote : Entity
{
    /// <summary>
    /// Gets the time the upvote was created 
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// The type of the upvote
    /// </summary>
    public int TypeId { get; private set; }

    /// <summary>
    /// The ID of the <see cref="User"/> that created the upvote.
    /// </summary>
    public int CreatedById { get; private set; }

#pragma warning disable CS8618
    /// <summary>
    /// Initalizes an instance of a <see cref="Upvote"/> object.
    /// </summary>
    protected Upvote()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initalizes an instance of a <see cref="Upvote"/> object.
    /// </summary>
    /// <param name="typeId">The type of the upvote</param>
    /// <param name="createdBy">The user creating the upvote</param>
    public Upvote(int typeId, User createdBy)
    {
        this.CreatedAtUtc = DateTime.UtcNow;
        this.TypeId = typeId;
        this.CreatedById = createdBy.Id;
    }
}