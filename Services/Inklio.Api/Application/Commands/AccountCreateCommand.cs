using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// Command used to register a new account
/// </summary>
public class AccountCreateCommand : IRequest<bool>
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
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password confirmation password to use when registering.
    /// </summary>
    [DataType(DataType.Password)]
    [JsonPropertyName("confirm_password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username to use when registering.
    /// </summary>
    [Required]
    [Username]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
}