using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Creates a tag for an object
/// </summary>
public class Tag : Entity
{
    /// <summary>
    /// Gets the value of the tag.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Gets the type of the tag.
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    /// Gets the ID of the user that created the tag. 
    /// </summary>
    public int CreatedById { get; private set; }

    /// <summary>
    /// Gets the time the Tag was created. 
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Initiliazes a new instance of a <see cref="Tag"/> object.
    /// </summary>
    /// <param name="createdById">The ID user that created the tag.</param>
    /// <param name="type">The type of the tag.</param>
    /// <param name="value">The value of the tag.</param>
    /// <exception cref="ArgumentException"></exception>
    public Tag(int createdById, string type, string value)
    {
        if (string.IsNullOrEmpty(type))
        {
            throw new ArgumentNullException($"'{nameof(type)}' cannot be null or empty.", nameof(type));
        }

        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException($"'{nameof(value)}' cannot be null or empty.", nameof(value));
        }

        this.CreatedById = createdById;
        this.Type = type;
        this.Value = value;
        this.CreatedAtUtc = DateTime.UtcNow;
    }
}