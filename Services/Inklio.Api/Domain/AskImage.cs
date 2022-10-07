using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Reperesents an image
/// </summary>
public class AskImage : Image
{
    /// <summary>
    /// Gets the parent <see cref="Ask"/> parent object ID.
    /// </summary>
    public int AskId { get; private set; }

    /// <summary>
    /// Gets the parent <see cref="Ask"/> object.
    /// </summary>
    public Ask Ask { get; private set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="AskImage"/> object.
    /// </summary>
#pragma warning disable CS8618
    protected AskImage()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="AskImage"/>
    /// </summary>
    /// <param name="ask">The ask associated with the blob</param>
    /// <param name="blobUrl">The url of the blob</param>
    /// <param name="contentType">The contet type of the blob</param>
    /// <param name="createdBy">The user that created the blob</param>
    /// <param name="name">The name of the image</param>
    /// <param name="sizeInBytes">The size in bytes of the blob</param>
    protected AskImage(Ask ask, Uri? blobUrl, string contentType, User createdBy, Guid name, long sizeInBytes)
        : base(ask, blobUrl, contentType, createdBy, name, sizeInBytes)
    {
        this.AskId = ask.Id;
        this.Ask = ask;
    }

    /// <summary>
    /// Creates an instance of a <see cref="AskImage"/>.
    /// </summary>
    /// <param name="ask">The ask assoiated with the blob</param>
    /// <param name="contentType">The type of the blob</param>
    /// <param name="createdBy">The user creating the blob</param>
    /// <param name="sizeInBytes">The size of the blob in bytes</param>
    /// <returns></returns>
    public static AskImage Create(Ask ask, string contentType, User createdBy, long sizeInBytes)
    {
        // TODO: Validate
        // 1. user can create blob
        // 2. blob content type is valid
        // 3. blob size is not too large

        var askBlob = new AskImage(ask, null, contentType, createdBy, Guid.NewGuid(), sizeInBytes);
        return askBlob;
    }
}