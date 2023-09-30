
namespace Inklio.Api.Domain;

/// <summary>
/// A comment that was made on a Delivery
/// </summary>
public class DeliveryComment : Comment
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
    /// Initializes a new instance of a <see cref="DeliveryComment"/> object.
    /// </summary>
#pragma warning disable CS8618
    private DeliveryComment()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="DeliveryComment"/>
    /// </summary>
    /// <param name="body">The body of the comment.</param>
    /// <param name="createdBy">The comment creator.</param>
    /// <param name="delivery">The parent <see cref="Delivery"/> object.</param>
    public DeliveryComment(string body, User createdBy, Delivery delivery) : base(delivery.Ask, body, createdBy)
    {
        this.Delivery = delivery;
        this.DeliveryId = delivery.Id;
    }
}