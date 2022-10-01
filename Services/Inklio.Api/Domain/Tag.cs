using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Creates a tag for an object
/// </summary>
public class Tag : Entity, IAggregateRoot
{
    /// <summary>
    /// The default type of a tag if one does not exist 
    /// </summary>
    public const string DefaultTagType = "general";

    /// <summary>
    /// The maximum number of characters a tag value or type can have. 
    /// </summary>
    public const int MaxTagLength = 32;

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

    /// <summary>
    /// Gets a collection of <see cref="Delivery"/> objects associated with the <see cref="Tag"/>.
    /// </summary>
    public IReadOnlyCollection<Delivery> Deliveries { get; private set; } = new List<Delivery>();

    /// <summary>
    /// Gets a collection of <see cref="AskTag"/> objects associated with the <see cref="Tag"/>.
    /// </summary>
    public IReadOnlyCollection<DeliveryTag> DeliveryTags { get; private set; } = new List<DeliveryTag>();

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
    protected Tag(User createdBy, string type, string value)
    {
        this.CreatedBy = createdBy;
        this.Type = type;
        this.Value = value;
        this.CreatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of a <see cref="Tag"/> object.
    /// </summary>
    /// <param name="createdBy">The user that created the tag.</param>
    /// <param name="type">The type of the tag.</param>
    /// <param name="value">The value of the tag.</param>
    /// <exception cref="ArgumentException"></exception>
    public static Tag Create(User createdBy, string type, string value)
    {
        // TODO - Validate User can create a new tag

        if (type.Length < 1 || type.Length > 32)
        {
            throw new InklioDomainException(400, $"Invalid tag. A tag type cannot be longer than {Tag.MaxTagLength} characters");
        }

        if (string.IsNullOrWhiteSpace(value) || value.Length < 1 || value.Length > Tag.MaxTagLength)
        {
            throw new InklioDomainException(400, $"Invalid tag. A tag must have a value and cannot be longer than {Tag.MaxTagLength} characters");
        }

        string tagType = string.IsNullOrWhiteSpace(type) ? Tag.DefaultTagType : type;
        string tagValue = value.ToLowerInvariant();
        return new Tag(createdBy, tagType, tagValue);
    }

    /// <summary>
    /// Converts the tag to a string in the format "tagType:tagValue"
    /// </summary>
    /// <returns>The tag to a string in the format "tagType:tagValue"</returns>
    public override string ToString()
    {
        return $"{this.Type}:{this.Value}";
    }
}