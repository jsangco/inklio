using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An Inklio Ask DTO
/// </summary>
public class Ask
{
    /// <summary>
    /// Gets or sets the ID of the ask.
    /// </summary>
    [JsonPropertyName("id")]
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Body of the Ask.
    /// </summary>
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can comment on the Ask.
    /// </summary>
    [JsonPropertyName("can_comment")]
    public bool CanComment { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not deliveries can be added.
    /// </summary>
    [JsonPropertyName("can_deliver")]
    public bool CanDeliver { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the ask.
    /// </summary>
    [JsonPropertyName("can_edit")]
    public bool CanEdit { get; set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the ask.
    /// </summary>
    [JsonPropertyName("can_flag")]
    public bool CanFlag { get; set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not can tag the ask.
    /// </summary>
    [JsonPropertyName("can_tag")]
    public bool CanTag { get; set; } = true;

    /// <summary>
    /// Gets or sets a collection of comments for the ask.
    /// </summary>
    [JsonPropertyName("comments")]
    public IEnumerable<Comment> Comments { get; set; } = Array.Empty<Comment>();

    /// <summary>
    /// Gets or sets the UTC time the ask was created.
    /// </summary>
    [JsonPropertyName("created_at_utc")]
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user that created the ask.
    /// </summary>
    [JsonPropertyName("created_by_id")]
    public int CreatedById { get; set; }

    /// <summary>
    /// Gets or sets the deliveries for the ask.
    /// </summary>
    [JsonPropertyName("deliveries")]
    public IEnumerable<Delivery> Deliveries { get; set; } = Array.Empty<Delivery>();

    /// <summary>
    /// Gets or sets the UTC time the ask was last edited.
    /// </summary>
    [JsonPropertyName("edited_at_utc")]
    public DateTime? EditedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the id of the user that edited the ask.
    /// </summary>    
    [JsonPropertyName("edited_by_id")]
    public int? EditedById { get; set; }

    /// <summary>
    /// Gets or sets the number of times an account was flagged.
    /// </summary>
    [JsonPropertyName("flag_count")]
    public int FlagCount { get; set; }

    /// <summary>
    /// Gets or sets the images of the ask.
    /// </summary>
    [JsonPropertyName("images")]
    public IEnumerable<AskImage> Images { get; set; } = Array.Empty<AskImage>();

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask is deleted.
    /// </summary>
    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has at least one delivery.
    /// </summary>
    [JsonPropertyName("is_delivered")]
    public bool IsDelivered { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has an accepted delivery.
    /// </summary>
    [JsonPropertyName("is_delivery_accepted")]
    public bool IsDeliveryAccepted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has been locked.
    /// </summary>
    [JsonPropertyName("is_locked")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask is NSFW.
    /// </summary>
    [JsonPropertyName("is_nsfw")]
    public bool IsNsfw { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask NSFL.
    /// </summary>
    [JsonPropertyName("is_nsfl")]
    public bool IsNsfl { get; set; }

    /// <summary>
    /// Gets or sets the UTC time that the ask was locked.
    /// </summary>
    [JsonPropertyName("locked_at_utc")]
    public DateTime LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the ask was saved.
    /// </summary>
    [JsonPropertyName("save_count")]
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets a collection of Tags for the ask.
    /// </summary>
    [JsonPropertyName("tags")]
    public IEnumerable<Tag> Tags { get; set; } = Array.Empty<Tag>();

    /// <summary>
    /// Gets or sets the Title of the Ask.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times the ask was upvoted.
    /// </summary>
    [JsonPropertyName("upvote_count")]
    public int UpvoteCount { get; set; }
    /// <summary>
    /// Gets or sets the number of times the ask has been viewed.
    /// </summary>

    [JsonPropertyName("view_count")]
    public int ViewCount { get; set; }
}