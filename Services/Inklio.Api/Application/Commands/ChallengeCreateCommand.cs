using System.Text.Json.Serialization;
using Inklio.Api.Domain;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

[DataContract]
public class ChallengeCreateCommand : IRequest<bool>
{
    /// <summary>
    /// The ask associated with the <see cref="Challenge"/>
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the type of the challenge.
    /// </summary>
    [DataMember(Name = "challengeType")]
    [JsonPropertyName("challengeType")]
    public ChallengeType ChallengeType { get; set; }

    /// <summary>
    /// The ID of the user creating the ask
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public UserId UserId { get; set; }

    /// <summary>
    /// Gets the UTC time the <see cref="Challenge"/> was ended.
    /// </summary>
    [DataMember(Name = "endAtUtc")]
    [JsonPropertyName("endAtUtc")]
    public DateTime EndAtUtc { get; set; }

    /// <summary>
    /// Gets the UTC time the <see cref="Challenge"/> was started.
    /// </summary>
    [DataMember(Name = "startAtUtc")]
    [JsonPropertyName("startAtUtc")]
    public DateTime StartAtUtc { get; set; }
}