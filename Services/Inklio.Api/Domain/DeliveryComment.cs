namespace Inklio.Api.Domain;

/// <summary>
/// A comment that was made on a Delivery
/// </summary>
public class DeliveryComment : Comment
{
    /// <summary>
    /// Gets or sets the ID of the parent Ask
    /// </summary>
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the parent Ask
    /// </summary>
    public Ask Ask { get; set; } = new Ask();

    /// <summary>
    /// Gets or sets the ID of the parent Delivery
    /// </summary>
    public int DeliveryId { get; set; }

    /// <summary>
    /// Gets or sets the parent Delivery
    /// </summary>
    public Delivery Delivery { get; set; } = new Delivery();
}