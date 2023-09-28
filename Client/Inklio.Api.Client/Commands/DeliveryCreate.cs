using System.Text.Json.Serialization;
using System.Text.Json;

namespace Inklio.Api.Client;

/// <summary>
/// An object to create a new delivery
/// </summary>
public class DeliveryCreate
{
    /// <summary>
    /// Gets or sets the Body of the Ask.
    /// </summary>
    [DataMember(Name = "body")]
    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets the content rating of the delivery.
    /// </summary>
    [DataMember(Name = "contentRating")]
    [JsonPropertyName("contentRating")]
    public byte ContentRating { get; set; }

    /// <summary>
    /// Gets or sets any images assoiated with the ask.
    /// </summary>
    [DataMember(Name = "images")]
    [JsonPropertyName("images")]
    public IEnumerable<byte[]>? Images { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery was created using Ai.
    /// </summary>
    [DataMember(Name = "isAi")]
    [JsonPropertyName("isAi")]
    public bool IsAi { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the delivery contains a spoiler.
    /// </summary>
    [DataMember(Name = "isSpoiler")]
    [JsonPropertyName("isSpoiler")]
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
    public IEnumerable<Tag> Tags { get; set; } = new Tag[] { };

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
        var deliveryCreateCopy = JsonSerializer.Deserialize<DeliveryCreate>(JsonSerializer.Serialize(this));
        this.Images = imagesTemp;
        if (deliveryCreateCopy is null)
        {
            throw new InvalidOperationException("Unable to serialize object");
        }

        string deliveryJson = JsonSerializer.Serialize(deliveryCreateCopy);

        content.Add(new StringContent(deliveryJson), "delivery");
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