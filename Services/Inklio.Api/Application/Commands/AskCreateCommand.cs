using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MediatR;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An request body to create a new ask
/// </summary>
public class AskCreateCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the Body of the Ask.
    /// </summary>
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask is NSFW.
    /// </summary>
    [JsonPropertyName("is_nsfw")]
    public bool IsNswf { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask NSFL.
    /// </summary>
    [JsonPropertyName("is_nsfl")]
    public bool IsNsfl { get; set; }

    /// <summary>
    /// Gets or sets the Title of the Ask.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or set the tags associated with the Ask
    /// </summary>
    [JsonPropertyName("tags")]
    public IEnumerable<Tag> Tags { get; set; } = new Tag[] { };

    /// <summary>
    /// Gets or sets the ID of the user
    /// </summary>
    [JsonIgnore]
    public int UserId { get; set; }
}