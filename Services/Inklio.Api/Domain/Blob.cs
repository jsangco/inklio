using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// The reference object mapping an Ask to a Tag.
/// </summary>
public class Blob
{
    /// <summary>
    /// Gets the Content-Type of the blob.
    /// </summary>
    public string ContentType { get; private set; }

    /// <summary>
    /// Gets the url of the blob.
    /// </summary>
    public Uri Url { get; private set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="Blob"/>
    /// </summary>
    /// <param name="contentType">The content type of the blob</param>
    /// <param name="url">The url of the blob</param>
    public Blob(string contentType, Uri url)
    {
        this.ContentType = contentType;
        this.Url = url;
    }
}