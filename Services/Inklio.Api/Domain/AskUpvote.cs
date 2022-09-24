using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An upvote for a <see cref="Ask"/> 
/// </summary>
public class AskUpvote : Upvote
{
    /// <summary>
    /// Gets the parent <see cref="Ask"/> parent object ID.
    /// </summary>
    public int AskId { get; private set; }

    /// <summary>
    /// Gets the parent <see cref="Ask"/> object.
    /// </summary>
    public Ask Ask { get; private set; }

    /// <summary>
    /// Initilaizes an instance of a <see cref="AskUpvote"/>
    /// </summary>
#pragma warning disable CS8618
    private AskUpvote() : base()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="AskUpvote"/>
    /// </summary>
    /// <param name="ask">The parent <see cref="Ask"/> object</param>
    /// <param name="typeId">The type of the upvote</param>
    /// <param name="createdBy">The Upvote creator</param>
    public AskUpvote(Ask ask, int typeId, User user) : base(typeId, user)
    {
        this.Ask = ask;
        this.AskId = ask.Id;
    }
}