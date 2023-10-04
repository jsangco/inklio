using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;

/// <summary>
/// Handler for deleting upvotes
/// </summary>
public class UpvoteDeleteCommandHandler : IRequestHandler<UpvoteDeleteCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes an instance of a new <see cref="UpvoteDeleteCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public UpvoteDeleteCommandHandler(
        IAskRepository askRepository,
        IUserRepository userRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Deletes an upvote to an existing ask or delivery if the upvote exists.
    /// </summary>
    /// <param name="request">The command to delete the Ask.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A flag indicating whether the upvote deletion was a success.</returns>
    public async Task<bool> Handle(UpvoteDeleteCommand request, CancellationToken cancellationToken)
    {
        DomainUser user = await this.userRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        DomainAsk ask = await this.askRepository.GetAskByIdAsync(request.AskId, cancellationToken);

        if (request.DeliveryId.HasValue)
        {
            if (request.CommentId.HasValue)
            {
                ask.DeleteDeliveryCommentUpvote(request.CommentId.Value, request.DeliveryId.Value, user);
            }
            else
            {
                ask.DeleteDeliveryUpvote(request.DeliveryId.Value, user);
            }
        }
        else
        {
            if (request.CommentId.HasValue)
            {
                ask.DeleteAskCommentUpvote(request.CommentId.Value, user);
            }
            else
            {
                ask.DeleteUpvote(user);
            }
        }

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}