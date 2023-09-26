namespace Inklio.Api.Application.Commands;

/// <summary>
/// An request body to create a new comment
/// </summary>
[DataContract]
public class UpvoteDeleteCommand : IRequest<bool>
{
    /// <summary>
    /// Gets or sets the ID of the ask to add the comment to
    /// </summary>
    public int AskId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the ask to add the comment to
    /// </summary>
    public int? DeliveryId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user creating the upvote.
    /// </summary>
    public UserId UserId { get; set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="UpvoteDeleteCommand"/> object.
    /// </summary>
    /// <param name="askId">The ID of the ask to remove the upvote from.</param>
    /// <param name="deliveryId">The ID of the ask to remove the upvote from.</param>
    /// <param name="userId">The ID of the user creating the upvote.</param>
    public UpvoteDeleteCommand(int askId, int deliveryId, UserId userId)
    {
        this.AskId = askId;
        this.DeliveryId = deliveryId;
        this.UserId = userId;
    }
}