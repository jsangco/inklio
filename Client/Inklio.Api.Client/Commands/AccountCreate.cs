using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Inklio.Api.Client;

/// <summary>
/// Command used to register a new account
/// </summary>
public class AccountCreate
{
    /// <summary>
    /// Gets or sets the email to use when registering.
    /// </summary>
    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password to use when registering.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 7)]
    [DataType(DataType.Password)]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password confirmation password to use when registering.
    /// </summary>
    [DataType(DataType.Password)]
    [JsonPropertyName("confirmPassword")]
    [Compare("Password", ErrorMessage = "The Password and the Confirmation Password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username to use when registering.
    /// </summary>
    [Required]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
}