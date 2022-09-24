using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An flag for a <see cref="Comment"/> 
/// </summary>
public class CommentFlag : Flag
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
    /// Initilaizes an instance of a <see cref="CommentFlag"/>
    /// </summary>
#pragma warning disable CS8618
    private CommentFlag() : base()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="CommentFlag"/>
    /// </summary>
    /// <param name="comment">The parent <see cref="Comment"/> object</param>
    /// <param name="typeId">The type of the Flag</param>
    /// <param name="createdBy">The Flag creator</param>
    public CommentFlag(Comment comment, int typeId, User user) : base(typeId, user)
    {
        this.Comment = comment;
        this.CommentId = Comment.Id;
    }
}