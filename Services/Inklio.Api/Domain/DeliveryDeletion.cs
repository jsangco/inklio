using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// A deletion for a <see cref="Delivery"/>.
/// </summary>
public class DeliveryDeletion : Deletion
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
    /// Initilaizes an instance of a <see cref="DeliveryDeletion"/>
    /// </summary>
#pragma warning disable CS8618
    private DeliveryDeletion() : base()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="DeliveryDeletion"/>
    /// </summary>
    /// <param name="delivery">The Delivery deleted</param>
    /// <param name="createdBy">The user that created the Delivery.</param>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public DeliveryDeletion(
        Delivery delivery,
        DeletionType deletionType,
        User createdBy,
        string internalComment,
        string userMessage) : base(createdBy, deletionType, internalComment, userMessage)
    {
        this.Delivery = delivery;
        this.DeliveryId = delivery.Id;
    }
}