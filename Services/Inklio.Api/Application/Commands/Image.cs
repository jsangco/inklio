using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

public class Image
{
    /// <summary>
    /// Gets or sets the url of the image.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}