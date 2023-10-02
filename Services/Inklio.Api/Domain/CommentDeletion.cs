using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// A deletion for a <see cref="Comment"/>.
/// </summary>
public class CommentDeletion : Deletion
{
    /// <summary>
    /// Gets the parent <see cref="Comment"/> parent object ID.
    /// </summary>
    public int CommentId { get; private set; }

    /// <summary>
    /// Gets the parent <see cref="Comment"/> object.
    /// </summary>
    public Comment Comment { get; private set; }

    /// <summary>
    /// Initilaizes an instance of a <see cref="CommentDeletion"/>
    /// </summary>
#pragma warning disable CS8618
    private CommentDeletion() : base()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="CommentDeletion"/>
    /// </summary>
    /// <param name="comment">The comment deleted</param>
    /// <param name="createdBy">The user that created the deletion.</param>
    /// <param name="deletionType">The type of the deletion.</param>
    /// <param name="internalComment">The internal comment on the deletion.</param>
    /// <param name="userMessage">The messeage shown to the user about the deletion.</param>
    public CommentDeletion(
        Comment comment,
        DeletionType deletionType,
        User createdBy,
        string internalComment,
        string userMessage) : base(createdBy, deletionType, internalComment, userMessage)
    {
        this.Comment = comment;
        this.CommentId = comment.Id;
    }
}