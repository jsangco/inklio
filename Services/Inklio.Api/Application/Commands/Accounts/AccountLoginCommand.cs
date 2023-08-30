using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Inklio.Api.Application.Commands.Accounts;

/// <summary>
/// Command sent when a user logs in
/// </summary>
public class AccountLoginCommand : IRequest<Account>
{
    /// <summary>
    /// Gets or sets the username or email used to login.
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password used to login
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indictaing whether to remember login status.
    /// </summary>
    [Display(Name = "Remember me?")]
    [JsonPropertyName("is_remember_me")]
    public bool IsRememberMe { get; set; }
}