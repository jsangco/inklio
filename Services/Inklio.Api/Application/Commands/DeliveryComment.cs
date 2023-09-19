using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

[DataContract]
public class DeliveryComment : Comment
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    [DataMember(Name = "deliveryId")]
    [JsonPropertyName("deliveryId")]
    public int DeliveryId { get; set; }
}