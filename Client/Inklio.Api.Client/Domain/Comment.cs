using System.Text.Json.Serialization;

namespace Inklio.Api.Client;

[DataContract]
public class Comment
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    [DataMember(Name = "id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Body of the comment.
    /// </summary>
    [DataMember(Name = "body")]
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the comment.
    /// </summary>
    [DataMember(Name = "can_edit")]
    [JsonPropertyName("can_edit")]
    public bool CanEdit { get; set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the comment.
    /// </summary>
    [DataMember(Name = "can_flag")]
    [JsonPropertyName("can_flag")]
    public bool CanFlag { get; set; } = true;

    /// <summary>
    /// Gets or sets the UTC time the comment was created.
    /// </summary>
    [DataMember(Name = "created_at_utc")]
    [JsonPropertyName("created_at_utc")]
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user that created the comment.
    /// </summary>
    [DataMember(Name = "created_by_id")]
    [JsonPropertyName("created_by_id")]
    public int CreatedById { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    [DataMember(Name = "edited_at_utc")]
    [JsonPropertyName("edited_at_utc")]
    public DateTime? EditedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the id of the user that edited the comment.
    /// </summary>
    [DataMember(Name = "edited_by_id")]
    [JsonPropertyName("edited_by_id")]
    public int? EditedById { get; set; }

    /// <summary>
    /// Gets or sets the number of times an account was flagged.
    /// </summary>
    [DataMember(Name = "flag_count")]
    [JsonPropertyName("flag_count")]
    public int FlagCount { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the comment is deleted.
    /// </summary>
    [DataMember(Name = "is_deleted")]
    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the comment has been locked.
    /// </summary>
    [DataMember(Name = "is_locked")]
    [JsonPropertyName("is_locked")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets the UTC time that the comment was locked.
    /// </summary>
    [DataMember(Name = "locked_at_utc")]
    [JsonPropertyName("locked_at_utc")]
    public DateTime LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the comment was saved.
    /// </summary>
    [DataMember(Name = "save_count")]
    [JsonPropertyName("save_count")]
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets the ID of ask for the comment.
    /// </summary>
    [DataMember(Name = "thread_id")]
    [JsonPropertyName("thread_id")]
    public int ThreadId { get; set; }

    /// <summary>
    /// Gets or sets the number of times the comment was upvoted.
    /// </summary>
    [DataMember(Name = "upvote_count")]
    [JsonPropertyName("upvote_count")]
    public int UpvoteCount { get; set; }
}