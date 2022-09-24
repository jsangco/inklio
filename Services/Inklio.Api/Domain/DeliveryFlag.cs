using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An Flag for a <see cref="Delivery"/> 
/// </summary>
public class DeliveryFlag : Flag
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
    /// Initilaizes an instance of a <see cref="DeliveryFlag"/>
    /// </summary>
#pragma warning disable CS8618
    private DeliveryFlag() : base()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="DeliveryComment"/>
    /// </summary>
    /// <param name="delivery">The parent <see cref="Delivery"/> object</param>
    /// <param name="typeId">The type of the Flag</param>
    /// <param name="createdBy">The Flag creator</param>
    public DeliveryFlag(Delivery delivery, int typeId, User user) : base(typeId, user)
    {
        this.Delivery = delivery;
        this.DeliveryId = delivery.Id;
    }
}