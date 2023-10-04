using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// A DTO reperesenting a User
/// </summary>
[DataContract]
public class User
{
    /// <summary>
    /// Gets the number of asks created by the user.
    /// </summary>
    [DataMember(Name ="askCount")]
    [JsonPropertyName("askCount")]
    public int AskCount { get; set; }

    /// <summary>
    /// Gets the asks created by the user.
    /// </summary>
    [DataMember(Name ="asks")]
    [JsonPropertyName("asks")]
    public IReadOnlyCollection<Ask> Asks { get; set; } = new List<Ask>();

    /// <summary>
    /// Gets the reputation earned by the user from asks.
    /// </summary>
    [DataMember(Name ="askReputation")]
    [JsonPropertyName("askReputation")]
    public int AskReputation { get; set; }

    /// <summary>
    /// Gets the comments created by the user.
    /// </summary>
    [DataMember(Name = "comments")]
    [JsonPropertyName("comments")]
    public IReadOnlyCollection<Comment> Comments { get; set; } = new List<Comment>();

    /// <summary>
    /// Gets or sets the reputation earned by the user from comments.
    /// </summary>
    [DataMember(Name = "commentReputation")]
    [JsonPropertyName("commentReputation")]
    public int CommentReputation { get; set; }

    /// <summary>
    /// Gets the UTC time the user was created.
    /// </summary>
    [DataMember(Name ="createdAtUtc")]
    [JsonPropertyName("createdAtUtc")]
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets the number of deliveries made by the user.
    /// </summary>
    [DataMember(Name = "deliveryCount")]
    [JsonPropertyName("deliveryCount")]
    public int DeliveryCount { get; set; }

    /// <summary>
    /// Gets the deliveries made by the user.
    /// </summary>
    [DataMember(Name ="deliveries")]
    [JsonPropertyName("deliveries")]
    public IReadOnlyCollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    /// <summary>
    /// Gets the reputation a user has earned by making deliveries.
    /// </summary>
    [DataMember(Name ="deliveryReputation")]
    [JsonPropertyName("deliveryReputation")]
    public int DeliveryReputation { get; set; }

    /// <summary>
    /// Gets the last time the user was active on the site.
    /// </summary>
    [DataMember(Name ="lastActivityAtUtc")]
    [JsonPropertyName("lastActivityAtUtc")]
    public DateTime LastActivityAtUtc { get; set; }

    /// <summary>
    /// Gets the last time the user logged in to the site.
    /// </summary>
    [DataMember(Name ="lastLoginAtUtc")]
    [JsonPropertyName("lastLoginAtUtc")]
    public DateTime LastLoginAtUtc { get; set; }

    /// <summary>
    /// Gets the reputation of the user including comments, asks, deliveries, etc...
    /// </summary>
    [DataMember(Name ="reputation")]
    [JsonPropertyName("reputation")]
    public int Reputation { get; set; }

    /// <summary>
    /// Gets the UserId of the user.
    /// </summary>
    [DataMember(Name ="userId")]
    [JsonPropertyName("userId")]
    public UserId UserId { get; set; }

    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    [DataMember(Name ="username")]
    [JsonPropertyName("username")]
    public string Username { get; set; } = "";
}