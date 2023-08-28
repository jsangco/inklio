using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// Command sent when a user forgets their password
/// </summary>
public class AccountForgetPasswordCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

}