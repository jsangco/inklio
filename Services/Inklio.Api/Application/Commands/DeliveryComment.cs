using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

public class DeliveryComment : Comment
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    [JsonPropertyName("delivery_id")]
    public int DeliveryId { get; set; }
}