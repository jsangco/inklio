namespace Inklio.Api.Domain;

/// <summary>
/// A comment that was made on an Ask
/// </summary>
public class AskComment : Comment
{
    /// <summary>
    /// Gets the parent <see cref="Ask"/> parent object ID.
    /// </summary>
    public int AskId { get; set; }

    /// <summary>
    /// Gets the parent <see cref="Ask"/> object.
    /// </summary>
    public Ask Ask { get; set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="AskComment"/> object.
    /// </summary>
#pragma warning disable CS8618
    private AskComment() : base()
#pragma warning restore CS8618
    {
    }

    /// <summary>
    /// Initilaizes an instance of a <see cref="AskComment"/>
    /// </summary>
    /// <param name="ask">The parent <see cref="Ask"/> object</param>
    /// <param name="body">The body of the comment</param>
    /// <param name="createdById">The id of the comment creator</param>
    public AskComment(Ask ask, string body, int createdById) : base(ask, body, createdById)
    {
        this.Ask = ask;
        this.AskId = ask.Id;
    }
}