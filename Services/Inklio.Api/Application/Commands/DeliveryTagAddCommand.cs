using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

public class DeliveryTagAddCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the ID of the ask to add the comment to
    /// </summary>
    [JsonIgnore]
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the delivery to add the comment to
    /// </summary>
    [JsonIgnore]
    public int DeliveryId { get; set; }

    /// <summary>
    /// Gets or sets the tag info.
    /// </summary>
    [JsonPropertyName("tag")]
    public Tag Tag { get; set; } = new();

    /// <summary>
    /// Gets or sets a flag indicating whether child deliveries should also be tagged.
    /// </summary>
    [JsonPropertyName("tag_deliveries")]
    public bool TagDeliveries { get; set; } = true;

    /// <summary>
    /// Gets or sets the ID of the user
    /// </summary>
    [JsonIgnore]
    public int UserId { get; set; }
}