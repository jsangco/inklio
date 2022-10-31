using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inklio.Auth.Models;

/// <summary>
/// The model used when a user forgets their password
/// </summary>
public class AccountForgetPassword
{
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

}