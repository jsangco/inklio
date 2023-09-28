using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MediatR;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An request body to create a new ask
/// </summary>
[DataContract]
public class AskCreateCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the Body of the Ask.
    /// </summary>
    [DataMember(Name = "body")]
    [JsonPropertyName("body")]
    [Required, MaxLength(40000)]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets the content rating of the ask.
    /// </summary>
    [DataMember(Name = "contentRating")]
    [JsonPropertyName("contentRating")]
    public byte ContentRating { get; set; }

    /// <summary>
    /// Gets or sets optional images to include in the ask
    /// </summary>
    [DataMember(Name = "images")]
    [JsonPropertyName("images")]
    public IEnumerable<IFormFile>? Images { get; set; } = null;

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask contains a spoiler.
    /// </summary>
    [DataMember(Name = "isSpoiler")]
    [JsonPropertyName("isSpoiler")]
    public bool IsSpoiler { get; set; }

    /// <summary>
    /// Gets or sets the Title of the Ask.
    /// </summary>
    [DataMember(Name = "title")]
    [JsonPropertyName("title")]
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or set the tags associated with the Ask
    /// </summary>
    [DataMember(Name = "tags")]
    [JsonPropertyName("tags")]
    public IEnumerable<Tag> Tags { get; set; } = new Tag[] { };

    /// <summary>
    /// Gets or sets the ID of the user
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public UserId UserId { get; set; }
}