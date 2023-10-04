using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An Inklio Ask DTO
/// </summary>
[DataContract]
public class Ask
{
    /// <summary>
    /// Gets or sets the ID of the ask.
    /// </summary>
    [DataMember(Name = "id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Body of the Ask.
    /// </summary>
    [DataMember(Name = "body")]
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can comment on the Ask.
    /// </summary>
    [DataMember(Name = "canComment")]
    [JsonPropertyName("canComment")]
    public bool CanComment { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not deliveries can be added.
    /// </summary>
    [DataMember(Name = "canDeliver")]
    [JsonPropertyName("canDeliver")]
    public bool CanDeliver { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the ask.
    /// </summary>
    [DataMember(Name = "canEdit")]
    [JsonPropertyName("canEdit")]
    public bool CanEdit { get; set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the ask.
    /// </summary>
    [DataMember(Name = "canFlag")]
    [JsonPropertyName("canFlag")]
    public bool CanFlag { get; set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not can tag the ask.
    /// </summary>
    [DataMember(Name = "canTag")]
    [JsonPropertyName("canTag")]
    public bool CanTag { get; set; } = true;

    /// <summary>
    /// Gets the content rating of the ask.
    /// </summary>
    [DataMember(Name = "contentRating")]
    [JsonPropertyName("contentRating")]
    public byte ContentRating { get; set; }

    /// <summary>
    /// Gets or sets the number of comments on an ask--includes the number
    /// of comments on child deliveries.
    /// </summary>
    [DataMember(Name = "commentCount")]
    [JsonPropertyName("commentCount")]
    public int CommentCount { get; set; }

    /// <summary>
    /// Gets or sets a collection of comments for the ask.
    /// </summary>
    [DataMember(Name = "comments")]
    [JsonPropertyName("comments")]
    public IEnumerable<AskComment> Comments { get; set; } = Array.Empty<AskComment>();

    /// <summary>
    /// Gets or sets the UTC time the ask was created.
    /// </summary>
    [DataMember(Name = "createdAtUtc")]
    [JsonPropertyName("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the username of the user that created the ask.
    /// </summary>
    [DataMember(Name = "createdBy")]
    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = "";

    /// <summary>
    /// Gets or sets the number of deliveries on the ask.
    /// </summary>
    [DataMember(Name = "deliveryCount")]
    [JsonPropertyName("deliveryCount")]
    public int DeliveryCount { get; set; }

    /// <summary>
    /// Gets or sets the deliveries for the ask.
    /// </summary>
    [DataMember(Name = "deliveries")]
    [JsonPropertyName("deliveries")]
    public IEnumerable<Delivery> Deliveries { get; set; } = Array.Empty<Delivery>();

    /// <summary>
    /// Gets or sets the UTC time the ask was last edited.
    /// </summary>
    [DataMember(Name = "editedAtUtc")]
    [JsonPropertyName("editedAtUtc")]
    public DateTime? EditedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the id of the user that edited the ask.
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
    /// Gets or sets the images of the ask.
    /// </summary>
    [DataMember(Name = "images")]
    [JsonPropertyName("images")]
    public IEnumerable<AskImage> Images { get; set; } = Array.Empty<AskImage>();

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask is deleted.
    /// </summary>
    [DataMember(Name = "isDeleted")]
    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has at least one delivery.
    /// </summary>
    [DataMember(Name = "isDelivered")]
    [JsonPropertyName("isDelivered")]
    public bool IsDelivered { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has an accepted delivery.
    /// </summary>
    [DataMember(Name = "isDeliveryAccepted")]
    [JsonPropertyName("isDeliveryAccepted")]
    public bool IsDeliveryAccepted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has been locked.
    /// </summary>
    [DataMember(Name = "isLocked")]
    [JsonPropertyName("isLocked")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask contains a spoiler.
    /// </summary>
    [DataMember(Name = "isSpoiler")]
    [JsonPropertyName("isSpoiler")]
    public bool IsSpoiler { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask has been upvoted by the user.
    /// </summary>
    [DataMember(Name = "isUpvoted")]
    [JsonPropertyName("isUpvoted")]
    public bool IsUpvoted { get; set; }

    /// <summary>
    /// Gets or sets the UTC time that the ask was locked.
    /// </summary>
    [DataMember(Name = "lockedAtUtc")]
    [JsonPropertyName("lockedAtUtc")]
    public DateTime? LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the hot rank for the ask.
    /// </summary>
    [DataMember(Name = "rankHot")]
    [JsonPropertyName("rankHot")]
    public int RankHot { get; set; }

    /// <summary>
    /// Gets or sets the number of times the ask was saved.
    /// </summary>
    [DataMember(Name = "saveCount")]
    [JsonPropertyName("saveCount")]
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets a collection of Tags for the ask.
    /// </summary>
    [DataMember(Name = "tags")]
    [JsonPropertyName("tags")]
    public IEnumerable<Tag> Tags { get; set; } = Array.Empty<Tag>();

    /// <summary>
    /// Gets or sets the Title of the Ask.
    /// </summary>
    [DataMember(Name = "title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times the ask was upvoted.
    /// </summary>
    [DataMember(Name = "upvoteCount")]
    [JsonPropertyName("upvoteCount")]
    public int UpvoteCount { get; set; }
    /// <summary>
    /// Gets or sets the number of times the ask has been viewed.
    /// </summary>

    [DataMember(Name = "viewCount")]
    [JsonPropertyName("viewCount")]
    public int ViewCount { get; set; }
}