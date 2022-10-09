using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MediatR;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An request body to create a new ask
/// </summary>
public class DeliveryCreateForm
{
    /// <summary>
    /// Gets or sets the Body of the Ask.
    /// </summary>
    [FromForm(Name = "body")]
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any images assoiated with the ask.
    /// </summary>
    [FromForm(Name = "images")]
    public IFormFile? Images { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask is NSFW.
    /// </summary>
    [FromForm(Name = "is_nsfw")]
    public bool IsNswf { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether or not the ask NSFL.
    /// </summary>
    [FromForm(Name = "is_nsfl")]
    public bool IsNsfl { get; set; }

    /// <summary>
    /// Gets or sets the Title of the Ask.
    /// </summary>
    [FromForm(Name = "title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or set the tags associated with the Ask
    /// </summary>
    [FromForm(Name = "tags")]
    public IEnumerable<Tag> Tags { get; set; } = new Tag[] { };
}