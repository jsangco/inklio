using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Inklio.Api.Client;

/// <summary>
/// A request body to create a new ask
/// </summary>
[DataContract]
public class AskCreate
{
    /// <summary>
    /// Gets or sets the Body of the Ask.
    /// </summary>
    [DataMember(Name = "body")]
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets optional images to include in the ask
    /// </summary>
    [DataMember(Name = "images")]
    [JsonPropertyName("images")]
    public IEnumerable<byte[]>? Images { get; set; } = null;

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask is NSFW.
    /// </summary>
    [DataMember(Name = "is_nsfw")]
    [JsonPropertyName("is_nsfw")]
    public bool IsNsfw { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask NSFL.
    /// </summary>
    [DataMember(Name = "is_nsfl")]
    [JsonPropertyName("is_nsfl")]
    public bool IsNsfl { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask is a spoiler.
    /// </summary>
    [DataMember(Name = "is_spoiler")]
    [JsonPropertyName("is_spoiler")]
    public bool IsSpoiler { get; set; }

    /// <summary>
    /// Gets or sets the Title of the Ask.
    /// </summary>
    [DataMember(Name = "title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or set the tags associated with the Ask
    /// </summary>
    [DataMember(Name = "tags")]
    [JsonPropertyName("tags")]
    [Required, MinLength(1)]
    public IEnumerable<Tag> Tags { get; set; } = new Tag[] { };

    /// <summary>
    /// Initializes an instance of an <see cref="AskCreate"/> object.
    /// </summary>
    public AskCreate()
    {
    }

    /// <summary>
    /// Initializes an instance of an <see cref="AskCreate"/> object.
    /// </summary>
    /// <param name="title">The title of the ask.</param>
    /// <param name="body">The body of the ask.</param>
    /// <param name="tags">The tags for the ask.</param>
    public AskCreate(string title, string body, params Tag[] tags)
    {
        this.Body = body;
        this.Title = title;
        this.Tags = tags;
    }

    /// <summary>
    /// Converts the object into a <see cref="MultipartFormDataContent"/> object that
    /// </summary>
    /// <returns></returns>
    public MultipartFormDataContent ToMultipartFormDataContent()
    {
        var content = new MultipartFormDataContent();

        // Create copy of object without the Images property which should not be part of the serialized JSON.
        var imagesTemp = this.Images;
        this.Images = null;
        var askCreateCopy = JsonSerializer.Deserialize<AskCreate>(JsonSerializer.Serialize(this));
        this.Images = imagesTemp;
        if (askCreateCopy is null)
        {
            throw new InvalidOperationException("Unable to serialize object");
        }

        string askJson = JsonSerializer.Serialize(askCreateCopy);

        content.Add(new StringContent(askJson), "ask");
        if (this.Images != null)
        {
            int i = 0;
            foreach (var image in this.Images)
            {
                content.Add(new ByteArrayContent(image), $"images", $"image{i}.png");
                ++i;
            }
        }
        return content;
    }
}