using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Inklio.Api.Models;

/// <summary>
/// An request body to create a new ask
/// </summary>
public class AskCreate
{
    /// <summary>
    /// Gets or sets the Body of the Ask.
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask is NSFW.
    /// </summary>
    public bool IsNswf { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask NSFL.
    /// </summary>
    public bool IsNsfl { get; set; }

    /// <summary>
    /// Gets or sets the Title of the Ask.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}