namespace Inklio.Api.Domain;

/// <summary>
/// A comment that was made on a Delivery
/// </summary>
public class DeliveryComment : Comment
{
    /// <summary>
    /// Gets or sets the parent Delivery
    /// </summary>
    public Delivery Delivery { get; set; } = new Delivery();
}