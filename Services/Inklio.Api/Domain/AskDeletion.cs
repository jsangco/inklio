using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// A deletion for a <see cref="Ask"/>
/// </summary>
public class AskDeletion : Deletion
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
    /// Initilaizes an instance of a <see cref="AskDeletion"/>
    /// </summary>
#pragma warning disable CS8618
    private AskDeletion() : base()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="AskDeletion"/>
    /// </summary>
    /// <param name="ask">The ask deleted</param>
    /// <param name="createdBy">The user that created the deletion.</param>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public AskDeletion(
        Ask ask,
        DeletionType deletionType,
        User createdBy,
        string internalComment,
        string userMessage) : base(createdBy, deletionType, internalComment, userMessage)
    {
        this.Ask = ask;
        this.AskId = ask.Id;
    }
}