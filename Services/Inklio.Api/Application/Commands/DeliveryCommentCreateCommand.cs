using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MediatR;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An request body to create a new comment
/// </summary>
[DataContract]
public class DeliveryCommentCreateCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the ID of the ask to add the comment to
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the ask to add the comment to
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int DeliveryId { get; set; }

    /// <summary>
    /// Gets or sets the Body of the Comment.
    /// </summary>
    [DataMember(Name = "body")]
    [JsonPropertyName("body")]
    [Required, MinLength(1), MaxLength(20000)]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the user
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public UserId UserId { get; set; }
}