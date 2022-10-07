namespace Inklio.Api.Domain;

/// <summary>
/// An image that was made on a Delivery
/// </summary>
public class DeliveryImage : Image
{
    /// <summary>
    /// Gets the parent <see cref="Delivery"/> object.
    /// </summary>
    public int DeliveryId { get; set; }

    /// <summary>
    /// Gets or sets the parent Delivery.
    /// </summary>
    public Delivery Delivery { get; set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="DeliveryImage"/> object.
    /// </summary>
#pragma warning disable CS8618
    protected DeliveryImage()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="DeliveryImage"/>
    /// </summary>
    /// <param name="blobUrl">The url of the blob</param>
    /// <param name="delivery">The delivery associated with the blob</param>
    /// <param name="contentType">The contet type of the blob</param>
    /// <param name="createdBy">The user that created the blob</param>
    /// <param name="name">The name of the image</param>
    /// <param name="sizeInBytes">The size in bytes of the blob</param>
    protected DeliveryImage(Uri? blobUrl, string contentType, User createdBy, Delivery delivery, Guid name, long sizeInBytes)
        : base(delivery.Ask, blobUrl, contentType, createdBy, name, sizeInBytes)
    {
        this.DeliveryId = delivery.Id;
        this.Delivery = delivery;
    }

    /// <summary>
    /// Creates an instance of a <see cref="DeliveryImage"/>.
    /// </summary>
    /// <param name="delivery">The delivery assoiated with the blob</param>
    /// <param name="contentType">The type of the blob</param>
    /// <param name="createdBy">The user creating the blob</param>
    /// <param name="sizeInBytes">The size of the blob in bytes</param>
    /// <returns></returns>
    public static DeliveryImage Create(string contentType, User createdBy, Delivery delivery, long sizeInBytes)
    {
        // TODO: Validate
        // 1. user can create blob
        // 2. blob content type is valid
        // 3. blob size is not too large

        var askBlob = new DeliveryImage(null, contentType, createdBy, delivery, Guid.NewGuid(), sizeInBytes);
        return askBlob;
    }
}