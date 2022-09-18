namespace Inklio.Api.Domain;

/// <summary>
/// Creates a tag for an object
/// </summary>
public class DeliveryTag : Tag
{
    /// <summary>
    /// Initiliazes a new instance of a <see cref="DeliveryTag"/> object.
    /// </summary>
    /// <param name="createdById">The ID user that created the tag.</param>
    /// <param name="type">The type of the tag.</param>
    /// <param name="value">The value of the tag.</param>
    public DeliveryTag(int createdById, string type, string value) : base(createdById, type, value)
    {
    }
}