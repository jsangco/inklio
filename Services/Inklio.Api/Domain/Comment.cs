using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Reperesents a Comment
/// </summary>
public class Comment : Entity, IAggregateRoot
{
    /// <summary>
    /// Gets or sets the Body of the Comment
    /// </summary>
    public string Body { get; set; } = string.Empty;
}