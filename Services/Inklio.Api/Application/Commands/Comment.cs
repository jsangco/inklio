using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

public class Comment
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the Body of the comment.
    /// </summary>
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the comment.
    /// </summary>
    [JsonPropertyName("can_edit")]
    public bool CanEdit { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the comment.
    /// </summary>
    [JsonPropertyName("can_flag")]
    public bool CanFlag { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the comment was created.
    /// </summary>
    [JsonPropertyName("created_at_utc")]
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user that created the comment.
    /// </summary>
    [JsonPropertyName("created_by_id")]
    public int CreatedById { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    [JsonPropertyName("edited_at_utc")]
    public DateTime EditedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the id of the user that edited the comment.
    /// </summary>    
    [JsonPropertyName("edited_by_id")]
    public int EditedById { get; set; }

    /// <summary>
    /// Gets or sets the number of times an account was flagged.
    /// </summary>
    [JsonPropertyName("flag_count")]
    public int FlagCount { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the comment is deleted.
    /// </summary>
    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the comment has been locked.
    /// </summary>
    [JsonPropertyName("is_locked")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets the UTC time that the comment was locked.
    /// </summary>
    [JsonPropertyName("locked_at_utc")]
    public DateTime LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the comment was saved.
    /// </summary>
    [JsonPropertyName("save_count")]
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets the number of times the comment was upvoted.
    /// </summary>
    [JsonPropertyName("upvote_count")]
    public int UpvoteCount { get; set; }
}