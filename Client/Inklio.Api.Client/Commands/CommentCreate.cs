using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using System.Text;

namespace Inklio.Api.Client;

/// <summary>
/// An object to create a comment on an ask or a delivery
/// </summary>
public class CommentCreate
{
    /// <summary>
    /// Initializes a new instance of a <see cref="CommentCreate"/> object.
    /// </summary>
    /// <param name="body">The body of the comment.</param>
    public CommentCreate(string body)
    {
        this.Body = body;
    }

    /// <summary>
    /// Gets or sets the Body of the Comment.
    /// </summary>
    [DataMember(Name = "body")]
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Converts the Comment into a <see cref="HttpContent/> object.
    /// </summary>
    /// <returns>An <see cref="HttpContent"/> object containing the content of the comment.</returns>
    public HttpContent ToHttpContent()
    {
        string json = JsonSerializer.Serialize(this);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
