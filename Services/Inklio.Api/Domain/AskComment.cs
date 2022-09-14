namespace Inklio.Api.Domain;

/// <summary>
/// A comment that was made on an Ask
/// </summary>
public class AskComment : Comment
{
    public Ask Ask { get; set; } = new Ask();
}