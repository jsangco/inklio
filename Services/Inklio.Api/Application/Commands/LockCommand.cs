using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Inklio.Api.Domain;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An command to lock a post
/// </summary>
[DataContract]
public class LockCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the ask ID for of the post to lock.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the reason the post was lock.
    /// </summary>
    public LockType LockType { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user locking the post.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public UserId EditedByUserId { get; set; }

    /// <summary>
    /// Gets or sets internal comments about the lock
    /// </summary>
    [StringLength(3000, MinimumLength = 0, ErrorMessage = "Internal comment is not within the required length.")]
    public string InternalComment { get; set; } = "";

    /// <summary>
    /// Gets or sets the message given to the user when the post is deleted.
    /// </summary>
    [StringLength(3000, MinimumLength = 0, ErrorMessage = "User message is not within the required length")]
    public string UserMessage { get; set; } = "";
}