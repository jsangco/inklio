using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// A DTO reperesenting a Tag 
/// </summary>
[DataContract]
public class Tag
{
    /// <summary>
    /// Gets or sets the ID of the tag.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int Id { get; set; }

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
    [Required(AllowEmptyStrings = false)]
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