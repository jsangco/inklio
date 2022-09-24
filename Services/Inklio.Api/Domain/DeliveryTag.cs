using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// The reference mapping of a <see cref="Delivery"/> to a <see cref="Tag"/>. 
/// </summary>
public class DeliveryTag
{
    /// <summary>
    /// Gets the ID of the referenced <see cref="DeliveryTag"/>.
    /// </summary>
    public int DeliveryId { get; private set; }

    /// <summary>
    /// Gets the referenced <see cref="DeliveryTag"/> entity.
    /// </summary>
    public Delivery Delivery { get; private set; }

    /// <summary>
    /// Gets the user that created the <see cref="DeliveryTag"/>. 
    /// </summary>
    public User CreatedBy { get; private set; }

    /// <summary>
    /// Gets the time that the <see cref="Tag"/> was added to the <see cref="DeliveryTag"/>
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the ID of the <see cref="Tag"/> referenced by the <see cref="DeliveryTag"/>.
    /// </summary>
    public int TagId { get; private set; }

    /// <summary>
    /// Gets the <see cref="Tag"/> associated with the <see cref="Delivery"/>.
    /// </summary>
    public Tag Tag { get; private set; }

#pragma warning disable CS8618
    /// <summary>
    /// Initializes a new instance of an <see cref="DeliveryTag"/> object.
    /// </summary>
    private DeliveryTag()
#pragma warning restore CS8618
    {
    }

    /// <summary>
    /// Initializes a new instance of an <see cref="DeliveryTag"/> object.
    /// </summary>
    /// <param name="delivery">The <see cref="Delivery"/> associated with a <see cref="Tag"/>.</param>
    /// <param name="createdBy">The <see cref="User"/> that associated the Delivery with the <see cref="Tag"/>.</param>
    /// <param name="tag">The <see cref="Tag"/> associated with a <see cref="Delivery"/>.</param>
    public DeliveryTag(Delivery delivery, User createdBy, Tag tag)
    {
        this.Delivery = delivery;
        this.DeliveryId = delivery.Id;
        this.CreatedBy = createdBy;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.Tag = tag;
        this.TagId = tag.Id;
    }
}