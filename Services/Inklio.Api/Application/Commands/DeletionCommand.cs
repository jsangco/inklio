using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Inklio.Api.Domain;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An request body to create a new comment
/// </summary>
[DataContract]
public class DeletionCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the ask ID for of the post to delete.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the comment Id of the post to delete.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int? CommentId { get; set; }

    /// <summary>
    /// Gets or sets the delivery ID of the post to delete.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int? DeliveryId { get; set; }

    /// <summary>
    /// Gets or sets the reason the post was deleted.
    /// </summary>
    public DeletionType DeletionType { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user deleting the post.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public UserId EditedById { get; set; }

    /// <summary>
    /// Gets or sets internal comments about the post deletion
    /// </summary>
    [StringLength(3000, MinimumLength = 0, ErrorMessage = "Internal comment is not within the required length.")]
    public string InternalComment { get; set; } = "";

    /// <summary>
    /// Gets or sets the message given to the user when the post is deleted.
    /// </summary>
    [StringLength(3000, MinimumLength = 0, ErrorMessage = "User message is not within the required length")]
    public string UserMessage { get; set; } = "";
}