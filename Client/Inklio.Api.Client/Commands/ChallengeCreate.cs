using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inklio.Api.Client;

[DataContract]
public class ChallengeCreate
{
    /// <summary>
    /// Gets or sets the type of the challenge.
    /// </summary>
    [DataMember(Name = "challengeType")]
    [JsonPropertyName("challengeType")]
    public ChallengeType ChallengeType { get; set; }

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

    /// <summary>
    /// Convert the <see cref="ChallengeCreate"/> into <see cref="HttpContent"/> that can be sent in a web request.
    /// </summary>
    /// <returns>The <see cref="ChallengeCreate"/> as <see cref="HttpContent"/>.</returns>
    internal HttpContent ToHttpContent()
    {
        string json = JsonSerializer.Serialize(this);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}