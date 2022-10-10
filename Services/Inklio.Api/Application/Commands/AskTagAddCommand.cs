using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

[DataContract]
public class AskTagAddCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the ID of the ask to add the comment to
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the tag info.
    /// </summary>
    [DataMember(Name = "tag")]
    [JsonPropertyName("tag")]
    public Tag Tag { get; set; } = new();

    /// <summary>
    /// Gets or sets a flag indicating whether child deliveries should also be tagged.
    /// </summary>
    [DataMember(Name = "tag_deliveries")]
    [JsonPropertyName("tag_deliveries")]
    public bool TagDeliveries { get; set; } = true;

    /// <summary>
    /// Gets or sets the ID of the user
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int UserId { get; set; }
}