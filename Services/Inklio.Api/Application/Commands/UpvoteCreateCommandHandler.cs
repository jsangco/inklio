using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;

/// <summary>
/// Handler for creating upvotes
/// </summary>
public class UpvoteCreateCommandHandler : IRequestHandler<UpvoteCreateCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes an instance of a new <see cref="UpvoteCreateCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public UpvoteCreateCommandHandler(
        IAskRepository askRepository,
        IUserRepository userRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Adds an upvote to an existing ask or delivery if the upvote exists.
    /// </summary>
    /// <param name="request">The command to create the upvote.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A flag indicating whether the upvote creation was a success.</returns>
    public async Task<bool> Handle(UpvoteCreateCommand request, CancellationToken cancellationToken)
    {
        User user = await this.userRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        DomainAsk ask = await this.askRepository.GetAskByIdAsync(request.AskId, cancellationToken);

        if (request.DeliveryId.HasValue)
        {
            ask.AddDeliveryUpvote(request.DeliveryId.Value, (int)UpvoteType.Basic, user);
        }
        else
        {
            ask.AddUpvote((int)UpvoteType.Basic, user);
        }

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}