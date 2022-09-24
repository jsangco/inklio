using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

public class AskTag
{
    public int AskId { get; private set; }
    public Ask Ask { get; private set; }
    public User CreatedBy { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public int TagId { get; private set; }
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
        // this.AskId = ask.Id;
        this.CreatedBy = createdBy;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.Tag = tag;
        // this.TagId = tag.Id;
    }
}