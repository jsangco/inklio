using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Inklio.Api.Domain;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An command to unlock a post
/// </summary>
[DataContract]
public class UnlockCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the ask ID for of the post to unlock.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user unlock the post.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public UserId EditedByUserId { get; set; }
}