using System.Text.Json.Serialization;

namespace Inklio.Api.Client;

[DataContract]
public class Challenge
{
    /// <summary>
    /// Gets or sets the ID of the <see cref="Challenge"/>.
    /// </summary>
    [DataMember(Name = "id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The ask associated with the <see cref="Challenge"/>
    /// </summary>
    [DataMember(Name = "ask")]
    [JsonPropertyName("ask")]
    public Ask Ask { get; private set; } = new Ask();

    /// <summary>
    /// Gets the type of the <see cref="Challenge"/>.
    /// </summary>
    [DataMember(Name = "challengeType")]
    [JsonPropertyName("challengeType")]
    public int ChallengeType { get; set; }
    
    /// <summary>
    /// Gets the UTC time the <see cref="Challenge"/> was created.
    /// </summary>
    [DataMember(Name = "createAtUtc")]
    [JsonPropertyName("createAtUtc")]
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the UTC time the <see cref="Challenge"/> was ended.
    /// </summary>
    [DataMember(Name = "endAtUtc")]
    [JsonPropertyName("endAtUtc")]
    public DateTime EndAtUtc { get; private set; }

    /// <summary>
    /// Gets the UTC time the <see cref="Challenge"/> was started.
    /// </summary>
    [DataMember(Name = "startAtUtc")]
    [JsonPropertyName("startAtUtc")]
    public DateTime StartAtUtc { get; private set; }

    /// <summary>
    /// Gets a enum indicating the current state of the challenge
    /// </summary>
    [DataMember(Name = "state")]
    [JsonPropertyName("state")]
    public int State { get; set; }

}