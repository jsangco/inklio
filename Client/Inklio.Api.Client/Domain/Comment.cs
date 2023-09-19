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
    [DataMember(Name = "canEdit")]
    [JsonPropertyName("canEdit")]
    public bool CanEdit { get; set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the comment.
    /// </summary>
    [DataMember(Name = "canFlag")]
    [JsonPropertyName("canFlag")]
    public bool CanFlag { get; set; } = true;

    /// <summary>
    /// Gets or sets the UTC time the comment was created.
    /// </summary>
    [DataMember(Name = "createdAtUtc")]
    [JsonPropertyName("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    [DataMember(Name = "editedAtUtc")]
    [JsonPropertyName("editedAtUtc")]
    public DateTime? EditedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the id of the user that edited the comment.
    /// </summary>
    [DataMember(Name = "editedById")]
    [JsonPropertyName("editedById")]
    public int? EditedById { get; set; }

    /// <summary>
    /// Gets or sets the number of times an account was flagged.
    /// </summary>
    [DataMember(Name = "flagCount")]
    [JsonPropertyName("flagCount")]
    public int FlagCount { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the comment is deleted.
    /// </summary>
    [DataMember(Name = "isDeleted")]
    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the comment has been locked.
    /// </summary>
    [DataMember(Name = "isLocked")]
    [JsonPropertyName("isLocked")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets the UTC time that the comment was locked.
    /// </summary>
    [DataMember(Name = "lockedAtUtc")]
    [JsonPropertyName("lockedAtUtc")]
    public DateTime LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the comment was saved.
    /// </summary>
    [DataMember(Name = "saveCount")]
    [JsonPropertyName("saveCount")]
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets the ID of ask for the comment.
    /// </summary>
    [DataMember(Name = "threadId")]
    [JsonPropertyName("threadId")]
    public int ThreadId { get; set; }

    /// <summary>
    /// Gets or sets the number of times the comment was upvoted.
    /// </summary>
    [DataMember(Name = "upvoteCount")]
    [JsonPropertyName("upvoteCount")]
    public int UpvoteCount { get; set; }
}