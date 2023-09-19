using System.Text.Json.Serialization;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

[DataContract]
public class AskComment : Comment
{
    /// <summary>
    /// Gets or sets the ID of the comment.
    /// </summary>
    [DataMember(Name = "askId")]
    [JsonPropertyName("askId")]
    public int AskId { get; set; }
}