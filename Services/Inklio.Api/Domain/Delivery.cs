using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Represents a Delivery object
/// </summary>
public class Delivery : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets or sets the Title of the Delivery
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Body of the Delivery
    /// </summary>
    public string Body { get; set; } = string.Empty;
}