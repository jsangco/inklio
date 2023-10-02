namespace Inklio.Api.Infrastructure.EFCore;

/// <summary>
/// The type of comment
/// </summary>
public enum DeletionClassType
{
    Unknown = 0,
    Deletion = 1,
    AskDeletion = 2,
    DeliveryDeletion = 3,
    CommentDeletion = 4,
}