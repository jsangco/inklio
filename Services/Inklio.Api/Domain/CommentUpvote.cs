using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An upvote for a <see cref="Comment"/> 
/// </summary>
public class CommentUpvote : Upvote
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
    /// Initilaizes an instance of a <see cref="CommentUpvote"/>
    /// </summary>
#pragma warning disable CS8618
    private CommentUpvote() : base()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="CommentUpvote"/>
    /// </summary>
    /// <param name="comment">The parent <see cref="Comment"/> object</param>
    /// <param name="typeId">The type of the upvote</param>
    /// <param name="createdBy">The Upvote creator</param>
    public CommentUpvote(Comment comment, int typeId, User user) : base(typeId, user)
    {
        this.Comment = comment;
        this.CommentId = Comment.Id;
    }
}