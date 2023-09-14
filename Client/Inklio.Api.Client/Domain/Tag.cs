using System.Text.Json.Serialization;

namespace Inklio.Api.Client;

/// <summary>
/// Creates a tag for an object
/// </summary>
public class Tag
{
    /// <summary>
    /// The default type of a tag if one does not exist
    /// </summary>
    public const string DefaultTagType = "general";

    /// <summary>
    /// Gets the time the Tag was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets the type of the tag.
    /// </summary>
    [DataMember(Name = "type")]
    [JsonPropertyName("type")]
    public string Type { get; set; } = "general";

    /// <summary>
    /// Gets the value of the tag.
    /// </summary>
    [DataMember(Name = "value")]
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
    /// <summary>
    /// Initiliazes a new instance of a <see cref="Tag"/> object.
    /// </summary>
    public Tag()
    {
    }

    /// <summary>
    /// Initiliazes a new instance of a <see cref="Tag"/> object.
    /// </summary>
    /// <param name="value">The value of the tag</param>
    public Tag(string value)
    {
        this.Value = value;
        this.Type = DefaultTagType;
    }

    /// <summary>
    /// Initiliazes a new instance of a <see cref="Tag"/> object.
    /// </summary>
    /// <param name="value">The value of the tag</param>
    /// <param name="type">The type of the tag</param>
    public Tag(string value, string type)
    {
        this.Value = value;
        this.Type = type;
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