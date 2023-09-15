using System.Text.Json.Serialization;

namespace Inklio.Api.Client;

/// <summary>
/// Represents an image.
/// </summary>
[DataContract]
public class Image
{
    /// <summary>
    /// Gets or sets the ID of the image.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the url of the image.
    /// </summary>
    [DataMember(Name = "url")]
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}