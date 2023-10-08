using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;

/// <summary>
/// Handler for Unlocking a post
/// </summary>
public class UnlockCommandHandler : IRequestHandler<UnlockCommand, bool>
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
    /// Initializes an instance of a new <see cref="UnlockCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public UnlockCommandHandler(
        IAskRepository askRepository,
        IUserRepository userRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Unlocks a post.
    /// </summary>
    /// <param name="request">The command of the Unlock.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A flag indicating whether the Unlock creation was a success.</returns>
    public async Task<bool> Handle(UnlockCommand request, CancellationToken cancellationToken)
    {
        DomainUser editor = await this.userRepository.GetByUserIdAsync(request.EditedByUserId, cancellationToken);
        DomainAsk ask = await this.askRepository.GetAskByIdAsync(request.AskId, null, true, cancellationToken);

        ask.Unlock(editor);

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}