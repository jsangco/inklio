using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An upvote entity used to track user upvotes.
/// </summary>
public class Flag : Entity
{
    /// <summary>
    /// Gets the time the Flag was created 
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// The type of the Flag
    /// </summary>
    public int TypeId { get; private set; }

    /// <summary>
    /// The ID of the <see cref="User"/> that created the Flag.
    /// </summary>
    public int createdById { get; private set; }

    /// <summary>
    /// The ID of the <see cref="User"/> that created the Flag.
    /// </summary>
    public User CreatedBy { get; private set; }

#pragma warning disable CS8618
    /// <summary>
    /// Initalizes an instance of a <see cref="Flag"/> object.
    /// </summary>
    protected Flag()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initalizes an instance of a <see cref="Flag"/> object.
    /// </summary>
    /// <param name="typeId">The type of the Flag</param>
    /// <param name="user">The user creating the Flag</param>
    public Flag(int typeId, User user)
    {
        this.CreatedAtUtc = DateTime.UtcNow;
        this.TypeId = typeId;
        this.CreatedBy = user;
        this.createdById = user.Id;
    }
}