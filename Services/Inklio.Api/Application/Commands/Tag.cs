using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// A DTO reperesenting a Tag 
/// </summary>
public class Tag
{
    /// <summary>
    /// Gets the type of the tag.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "general";

    /// <summary>
    /// Gets the value of the tag.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Converts the tag to a string in the format "tagType:tagValue"
    /// </summary>
    /// <returns>The tag to a string in the format "tagType:tagValue"</returns>
    public override string ToString()
    {
        return $"{this.Type}:{this.Value}";
    }
}