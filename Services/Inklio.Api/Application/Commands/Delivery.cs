using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inklio.Api.Application.Commands;

[DataContract]
public class Delivery
{
    /// <summary>
    /// Gets or sets the ID of the delivery.
    /// </summary>
    [DataMember(Name = "id")]
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the Body of the delivery.
    /// </summary>
    [DataMember(Name = "body")]
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets the content rating of the delivery.
    /// </summary>
    [DataMember(Name = "contentRating")]
    [JsonPropertyName("contentRating")]
    public byte ContentRating { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can comment on the delivery.
    /// </summary>
    [DataMember(Name = "canComment")]
    [JsonPropertyName("canComment")]
    public bool CanComment { get; set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can edit the delivery.
    /// </summary>
    [DataMember(Name = "canEdit")]
    [JsonPropertyName("canEdit")]
    public bool CanEdit { get; set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not a user can flag the delivery.
    /// </summary>
    [DataMember(Name = "canFlag")]
    [JsonPropertyName("canFlag")]
    public bool CanFlag { get; set; } = true;

    /// <summary>
    /// Gets or sets a flag indicating whether or not can tag the delivery.
    /// </summary>
    [DataMember(Name = "canTag")]
    [JsonPropertyName("canTag")]
    public bool CanTag { get; set; } = true;

    /// <summary>
    /// Gets or sets the rank the delivery received at the end of a challenge.
    /// </summary>
    [DataMember(Name = "challengeDeliveryRank")]
    [JsonPropertyName("challengeDeliveryRank")]
    public ChallengeDeliveryRank? ChallengeDeliveryRank { get; set; }

    /// <summary>
    /// Gets or sets the comments for the delivery.
    /// </summary>
    [DataMember(Name = "comments")]
    [JsonPropertyName("comments")]
    public IEnumerable<DeliveryComment> Comments { get; set; } = Array.Empty<DeliveryComment>();

    /// <summary>
    /// Gets or sets the UTC time the delivery was created.
    /// </summary>
    [DataMember(Name = "createdAtUtc")]
    [JsonPropertyName("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the user that created the delivery.
    /// </summary>
    [DataMember(Name = "createdBy")]
    [JsonPropertyName("createdBy")]
    public string CreatedBy { get; set; } = "";

    /// <summary>
    /// Gets or sets the UTC time the delivery was last edited.
    /// </summary>
    [DataMember(Name = "editedAtUtc")]
    [JsonPropertyName("editedAtUtc")]
    public DateTime? EditedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the id of the user that edited the delivery.
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
    /// Gets or sets the images of the delivery.
    /// </summary>
    [DataMember(Name = "images")]
    [JsonPropertyName("images")]
    public IEnumerable<DeliveryImage> Images { get; set; } = Array.Empty<DeliveryImage>();

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery is ai generated.
    /// </summary>
    [DataMember(Name = "isAi")]
    [JsonPropertyName("isAi")]
    public bool IsAi { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery is deleted.
    /// </summary>
    [DataMember(Name = "isDeleted")]
    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery has at least one delivery.
    /// </summary>
    [DataMember(Name = "isDelivered")]
    [JsonPropertyName("isDelivered")]
    public bool IsDelivered { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery has an accepted delivery.
    /// </summary>
    [DataMember(Name = "isDeliveryAccepted")]
    [JsonPropertyName("isDeliveryAccepted")]
    public bool IsDeliveryAccepted { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery has been locked.
    /// </summary>
    [DataMember(Name = "isLocked")]
    [JsonPropertyName("isLocked")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery contains a spoiler.
    /// </summary>
    [DataMember(Name = "isSpoiler")]
    [JsonPropertyName("isSpoiler")]
    public bool IsSpoiler { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery has been by upvoted by the user.
    /// </summary>
    [DataMember(Name = "isUpvoted")]
    [JsonPropertyName("isUpvoted")]
    public bool IsUpvoted { get; set; }

    /// <summary>
    /// Gets or sets the UTC time that the delivery was locked.
    /// </summary>
    [DataMember(Name = "lockedAtUtc")]
    [JsonPropertyName("lockedAtUtc")]
    public DateTime? LockedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of times the delivery was saved.
    /// </summary>
    [DataMember(Name = "saveCount")]
    [JsonPropertyName("saveCount")]
    public int SaveCount { get; set; }

    /// <summary>
    /// Gets or sets a collection of Tags for the delivery.
    /// </summary>
    [DataMember(Name = "tags")]
    [JsonPropertyName("tags")]
    public IEnumerable<Tag> Tags { get; set; } = Array.Empty<Tag>();

    /// <summary>
    /// Gets or sets the Title of the delivery.
    /// </summary>
    [DataMember(Name = "title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times the delivery was upvoted.
    /// </summary>
    [DataMember(Name = "upvoteCount")]
    [JsonPropertyName("upvoteCount")]
    public int UpvoteCount { get; set; }

    /// <summary>
    /// Gets or sets the number of times the delivery has been viewed.
    /// </summary>
    [DataMember(Name = "viewCount")]
    [JsonPropertyName("viewCount")]
    public int ViewCount { get; set; }
}