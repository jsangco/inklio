using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;

/// <summary>
/// Handler for locking a post
/// </summary>
public class LockCommandHandler : IRequestHandler<LockCommand, bool>
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
    /// Initializes an instance of a new <see cref="LockCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public LockCommandHandler(
        IAskRepository askRepository,
        IUserRepository userRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Locks an ask.
    /// </summary>
    /// <param name="request">The lock command.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A flag indicating whether the Lock creation was a success.</returns>
    public async Task<bool> Handle(LockCommand request, CancellationToken cancellationToken)
    {
        DomainUser editor = await this.userRepository.GetByUserIdAsync(request.EditedByUserId, cancellationToken);
        DomainAsk ask = await this.askRepository.GetAskByIdAsync(request.AskId, null, true, cancellationToken);

        ask.Lock(editor, request.LockType, request.InternalComment, request.UserMessage);

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}