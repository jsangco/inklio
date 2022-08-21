using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Domain reperesentation of an Ask
/// </summary>
public class Ask : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets or sets the Title of the Ask
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Body of the Ask
    /// </summary>
    public string Body { get; set; } = string.Empty;

    public IEnumerable<Comment> Comments { get; set; } = Array.Empty<Comment>();

    public IEnumerable<Delivery> Deliveries { get; set; } = Array.Empty<Delivery>();
}