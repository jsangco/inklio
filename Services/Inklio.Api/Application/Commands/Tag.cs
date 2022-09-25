using Inklio.Api.SeedWork;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// A DTO reperesenting a Tag 
/// </summary>
public class Tag
{
    /// <summary>
    /// Gets the type of the tag.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets the value of the tag.
    /// </summary>
    public string Value { get; set; } = string.Empty;

}