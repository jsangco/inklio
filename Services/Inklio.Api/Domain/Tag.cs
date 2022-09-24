using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Creates a tag for an object
/// </summary>
public class Tag : Entity
{
    /// <summary>
    /// Gets a collection of <see cref="Ask"/> objects associated with the <see cref="Tag"/>.
    /// </summary>
    public IReadOnlyCollection<Ask> Asks { get; private set; } = new List<Ask>();

    /// <summary>
    /// Gets a collection of <see cref="AskTag"/> objects associated with the <see cref="Tag"/>.
    /// </summary>
    public IReadOnlyCollection<AskTag> AskTags { get; private set; } = new List<AskTag>();

    /// <summary>
    /// Gets the ID of the user that created the tag. 
    /// </summary>
    public User CreatedBy { get; private set; }

    /// <summary>
    /// Gets the time the Tag was created. 
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    // TODO
    // TODO
    // TODO
    // TODO
    public List<Delivery> Deliveries { get; set; } = new List<Delivery>();

    /// <summary>
    /// Gets the type of the tag.
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    /// Gets the value of the tag.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Initiliazes a new instance of a <see cref="Tag"/> object.
    /// </summary>
    private Tag()
    {
        this.CreatedBy = new User("empty username");
        this.Value = "empty tag";
        this.Type = "general tag";
    }
    
    /// <summary>
    /// Initiliazes a new instance of a <see cref="Tag"/> object.
    /// </summary>
    /// <param name="createdBy">The user that created the tag.</param>
    /// <param name="type">The type of the tag.</param>
    /// <param name="value">The value of the tag.</param>
    /// <exception cref="ArgumentException"></exception>
    public Tag(User createdBy, string type, string value)
    {
        if (string.IsNullOrWhiteSpace(type) || type.Length < 1 || type.Length > 32)
        {
            throw new ArgumentNullException($"'{nameof(type)}' cannot be null or empty.", nameof(type));
        }

        if (string.IsNullOrWhiteSpace(value) || type.Length < 1 || type.Length > 32)
        {
            throw new ArgumentNullException($"'{nameof(value)}' cannot be null or empty.", nameof(value));
        }

        this.CreatedBy = createdBy;
        this.Type = type;
        this.Value = value;
        this.CreatedAtUtc = DateTime.UtcNow;
    }
}