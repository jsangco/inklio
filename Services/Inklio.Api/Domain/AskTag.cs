using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// The referenc object mapping an Ask to a Tag 
/// </summary>
public class AskTag
{
    /// <summary>
    /// Gets the ID of the referenced <see cref="AskTag"/>.
    /// </summary>
    public int AskId { get; private set; }

    /// <summary>
    /// Gets the referenced <see cref="AskTag"/> entity.
    /// </summary>
    public Ask Ask { get; private set; }

    /// <summary>
    /// Gets the user that created the <see cref="AskTag"/>. 
    /// </summary>
    public User CreatedBy { get; private set; }

    /// <summary>
    /// Gets the time that the <see cref="Tag"/> was added to the <see cref="AskTag"/>
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the ID of the <see cref="Tag"/> referenced by the <see cref="AskTag"/>.
    /// </summary>
    public int TagId { get; private set; }

    /// <summary>
    /// Gets the <see cref="Tag"/> associated with the <see cref="Ask"/>.
    /// </summary>
    public Tag Tag { get; private set; }

#pragma warning disable CS8618
    /// <summary>
    /// Initializes a new instance of an <see cref="AskTag"/> object.
    /// </summary>
    private AskTag()
#pragma warning restore CS8618
    {
    }

    /// <summary>
    /// Initializes a new instance of an <see cref="AskTag"/> object.
    /// </summary>
    /// <param name="ask">The <see cref="Ask"/> associated with a <see cref="Tag"/>.</param>
    /// <param name="createdBy">The <see cref="User"/> that associated the ask with the <see cref="Tag"/>.</param>
    /// <param name="tag">The <see cref="Tag"/> associated with a <see cref="Ask"/>.</param>
    public AskTag(Ask ask, User createdBy, Tag tag)
    {
        this.Ask = ask;
        this.AskId = ask.Id;
        this.CreatedBy = createdBy;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.Tag = tag;
        this.TagId = tag.Id;
    }
}