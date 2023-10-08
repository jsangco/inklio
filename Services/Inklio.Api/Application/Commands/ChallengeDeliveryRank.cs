using System.Text.Json.Serialization;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// The rank the delivery received at the end of a challenge.
/// </summary>
public class ChallengeDeliveryRank
{
    /// <summary>
    /// Gets the rank.
    /// </summary>
    [DataMember(Name = "rank")]
    [JsonPropertyName("rank")]
    public int Rank { get; set; }
}