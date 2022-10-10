using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MediatR;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An request body to create a new Delivery
/// </summary>
[DataContract]
public class DeliveryCreateCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the ID of the Ask
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the Body of the Delivery.
    /// </summary>
    [DataMember(Name = "body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether to include the parent tags when creating the delivery.
    /// </summary>
    public bool IncludeAskTags { get; set; } = true;

    /// <summary>
    /// Gets or set the tags associated with the Delivery
    /// </summary>
    [DataMember(Name = "images")]
    [JsonPropertyName("images")]
    public IEnumerable<IFormFile>? Images { get; set; } = null;

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery is NSFW.
    /// </summary>
    [DataMember(Name = "is_nsfw")]
    [JsonPropertyName("is_nsfw")]
    public bool IsNswf { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery NSFL.
    /// </summary>
    [DataMember(Name = "is_nsfl")]
    [JsonPropertyName("is_nsfl")]
    public bool IsNsfl { get; set; }

    /// <summary>
    /// Gets or sets the Title of the Delivery.
    /// </summary>
    [DataMember(Name = "title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or set the tags associated with the Delivery
    /// </summary>
    [DataMember(Name = "tags")]
    [JsonPropertyName("tags")]
    public IEnumerable<Tag> Tags { get; set; } = new Tag[] { };

    /// <summary>
    /// Gets or sets the ID of the user
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int UserId { get; set; }
}