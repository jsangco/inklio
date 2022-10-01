using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

public class Delivery
{
    /// <summary>
    /// Gets or sets the ID of the delivery.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the Body of the delivery.
    /// </summary>
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can comment on the delivery.
    /// </summary>
    [JsonPropertyName("can_comment")]
    public bool CanComment { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not deliveries can be added.
    /// </summary>
    [JsonPropertyName("can_deliver")]
    public bool CanDeliver { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the delivery.
    /// </summary>
    [JsonPropertyName("can_edit")]
    public bool CanEdit { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the delivery.
    /// </summary>
    [JsonPropertyName("can_flag")]
    public bool CanFlag { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not can tag the delivery.
    /// </summary>
    [JsonPropertyName("can_tag")]
    public bool CanTag { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the delivery was created.
    /// </summary>
    [JsonPropertyName("created_at_utc")]
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user that created the delivery.
    /// </summary>
    [JsonPropertyName("created_by_id")]
    public int CreatedById { get; set; }

    /// <summary>
    /// Gets or sets the UTC time the delivery was last edited.
    /// </summary>
    [JsonPropertyName("edited_at_utc")]
    public DateTime EditedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the id of the user that edited the delivery.
    /// </summary>    
    [JsonPropertyName("edited_by_id")]
    public int EditedById { get; set; }

    /// <summary>
    /// Gets or sets the number of times an account was flagged.
    /// </summary>
    [JsonPropertyName("flag_count")]
    public int FlagCount { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery is deleted.
    /// </summary>
    [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery has at least one delivery.
    /// </summary>
    [JsonPropertyName("is_delivered")]
    public bool IsDelivered { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery has an accepted delivery.
    /// </summary>
    [JsonPropertyName("is_delivery_accepted")]
    public bool IsDeliveryAccepted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery has been locked.
    /// </summary>
    [JsonPropertyName("is_locked")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery is NSFW.
    /// </summary>
    [JsonPropertyName("is_nsfw")]
    public bool IsNsfw { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery NSFL.
    /// </summary>
    [JsonPropertyName("is_nsfl")]
    public bool IsNsfl { get; set; }

    /// <summary>
    /// Gets or sets the UTC time that the delivery was locked.
    /// </summary>
    [JsonPropertyName("locked_at_utc")]
    public DateTime LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the delivery was saved.
    /// </summary>
    [JsonPropertyName("save_count")]
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets a collection of Tags for the ask.
    /// </summary>
    [JsonPropertyName("tags")]
    public IEnumerable<Tag> Tags { get; set; } = Array.Empty<Tag>();

    /// <summary>
    /// Gets or sets the Title of the delivery.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times the delivery was upvoted.
    /// </summary>
    [JsonPropertyName("upvote_count")]
    public int UpvoteCount { get; set; }

    /// <summary>
    /// Gets or sets the number of times the delivery has been viewed.
    /// </summary>
    [JsonPropertyName("view_count")]
    public int ViewCount { get; set; }
}