using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MediatR;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// An request body to create a new ask
/// </summary>
public class AskCreateForm
{
    /// <summary>
    /// Gets or sets a JSON string containing the Ask data. The string should be deserializable into
    /// a <see cref="AskCreateCommand"/> object.
    /// </summary>
    [FromForm(Name = "ask")]
    public string Ask { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets any images assoiated with the ask.
    /// </summary>
    [FromForm(Name = "images")]
    public IEnumerable<IFormFile>? Images { get; set; }
}