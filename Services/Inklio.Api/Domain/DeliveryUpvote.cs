using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An upvote for a <see cref="Delivery"/> 
/// </summary>
public class DeliveryUpvote : Upvote
{
    /// <summary>
    /// Gets the parent <see cref="Delivery"/> parent object ID.
    /// </summary>
    public int DeliveryId { get; private set; }

    /// <summary>
    /// Gets the parent <see cref="Delivery"/> object.
    /// </summary>
    public Delivery Delivery { get; private set; }

    /// <summary>
    /// Initilaizes an instance of a <see cref="DeliveryUpvote"/>
    /// </summary>
#pragma warning disable CS8618
    private DeliveryUpvote() : base()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="DeliveryComment"/>
    /// </summary>
    /// <param name="delivery">The parent <see cref="Delivery"/> object</param>
    /// <param name="typeId">The type of the upvote</param>
    /// <param name="createdBy">The Upvote creator</param>
    public DeliveryUpvote(Delivery delivery, int typeId, User createdBy) : base(typeId, createdBy)
    {
        this.Delivery = delivery;
        this.DeliveryId = delivery.Id;
    }
}