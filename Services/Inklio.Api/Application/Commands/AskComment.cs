using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

public class AskComment : Comment
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    [JsonPropertyName("ask_id")]
    public int AskId { get; set; }
}