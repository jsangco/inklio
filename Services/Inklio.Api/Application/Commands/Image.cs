using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

[DataContract]
public class Image
{
    /// <summary>
    /// Gets or sets the ID of the image.
    /// </summary>
    [DataMember(Name = "id")]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the url of the image.
    /// </summary>
    [DataMember(Name = "url")]
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}