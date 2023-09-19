using System.Text.Json.Serialization;

namespace Inklio.Api.Application.Commands.Accounts;

/// <summary>
/// The account object that represents the logged in user information.
/// </summary>
public class Account
{
    /// <summary>
    /// Gets or sets the ID of the <see cref="Account"/>.
    /// </summary>
    [DataMember(Name = "id")]
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    /// <summary>
    /// Gets or sets the roles of the <see cref="Account"/>.
    /// </summary>
    [DataMember(Name = "roles")]
    [JsonPropertyName("roles")]
    public IEnumerable<string> Roles { get; set; } = new string[] { };

    /// <summary>
    /// Gets or sets the Username of the <see cref="Account"/>.
    /// </summary>
    [DataMember(Name = "username")]
    [JsonPropertyName("username")]
    public string Username { get; set; } = "";

    /// <summary>
    /// Gets or sets a flag indicating whether the user has confirmed their email.
    /// </summary>
    [DataMember(Name = "isEmailVerified")]
    [JsonPropertyName("isEmailVerified")]
    public bool IsEmailVerified { get; set; } = false;
}