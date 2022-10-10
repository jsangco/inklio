using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

[DataContract]
public class DeliveryComment : Comment
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    [DataMember(Name = "delivery_id")]
    [JsonPropertyName("delivery_id")]
    public int DeliveryId { get; set; }
}