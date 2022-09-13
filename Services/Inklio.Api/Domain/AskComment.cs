namespace Inklio.Api.Domain;

/// <summary>
/// A comment that was made on an Ask
/// </summary>
public class AskComment : Comment
{
    /// <summary>
    /// Gets or sets the ID of the parent Ask
    /// </summary>
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the parent Ask
    /// </summary>
    public Ask Ask { get; set; } = new Ask();
}