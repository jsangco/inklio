namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// The type of comment
/// </summary>
public enum UpvoteClassType
{
    Unknown = 0,
    Upvote = 1,
    AskUpvote = 2,
    DeliveryUpvote = 3,
    CommentUpvote = 4,
}