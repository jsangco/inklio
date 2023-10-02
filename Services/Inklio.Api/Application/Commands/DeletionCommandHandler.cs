using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;

/// <summary>
/// Handler for creating deletions
/// </summary>
public class DeletionCommandHandler : IRequestHandler<DeletionCommand, bool>
{
    /// <summary>
    /// The ask repository.
    /// </summary>
    private readonly IAskRepository askRepository;

    /// <summary>
    /// The user repository.
    /// </summary>
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initializes an instance of a new <see cref="DeletionCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public DeletionCommandHandler(
        IAskRepository askRepository,
        IUserRepository userRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Adds an Deletion to an existing ask or delivery if the Deletion exists.
    /// </summary>
    /// <param name="request">The command to create the Deletion.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A flag indicating whether the Deletion creation was a success.</returns>
    public async Task<bool> Handle(DeletionCommand request, CancellationToken cancellationToken)
    {
        User editor = await this.userRepository.GetByUserIdAsync(request.EditedById, cancellationToken);
        DomainAsk ask = await this.askRepository.GetAnyAskByIdAsync(request.AskId, cancellationToken);

        if (request.DeliveryId.HasValue)
        {
            if (request.CommentId.HasValue)
            {
                ask.DeleteDeliveryComment(
                    request.CommentId.Value,
                    request.DeliveryId.Value,
                    request.DeletionType,
                    editor,
                    request.InternalComment,
                    request.UserMessage);
            }
            else
            {
                ask.DeleteDelivery(
                    request.DeliveryId.Value,
                    request.DeletionType,
                    editor,
                    request.InternalComment,
                    request.UserMessage);
            }
        }
        else
        {
            if (request.CommentId.HasValue)
            {
                ask.DeleteAskComment(
                    request.CommentId.Value,
                    request.DeletionType,
                    editor,
                    request.InternalComment,
                    request.UserMessage);
            }
            else
            {
                ask.Delete(
                    request.DeletionType,
                    editor,
                    request.InternalComment,
                    request.UserMessage);
            }
        }

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}