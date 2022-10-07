using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using CommandTag = Inklio.Api.Application.Commands.Tag;
using DomainTag = Inklio.Api.Domain.Tag;

/// <summary>
/// A handler that adds a Tag to an Ask. 
/// </summary>
public class AskTagAddCommandHandler : IRequestHandler<AskTagAddCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initialize a new instance of an <see cref="AskTagAddCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AskTagAddCommandHandler(IAskRepository askRepository, IUserRepository userRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Add a tag to an Ask.
    /// </summary>
    /// <param name="request">The command to add the tag to the Ask.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A flag indicating whether the tag addition was a success.</returns>
    public async Task<bool> Handle(AskTagAddCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userRepository.GetByIdAsync(request.UserId, cancellationToken);
        var ask = await this.askRepository.GetAskByIdAsync(request.AskId, cancellationToken);

        DomainTag tag = this.GetOrCreateTag(request.Tag, user);

        ask.AddTag(request.TagDeliveries, user, tag);

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Gets a tag from the repository. If a tag does not exist, it is created.
    /// </summary>
    /// <param name="tag">The tag to get.</param>
    /// <param name="user">The user creating getting or creating the tags</param>
    /// <returns>A collection of all relevant Tags that were retrieved from the repository.</returns>
    private DomainTag GetOrCreateTag(CommandTag tag, User user)
    {
        this.askRepository.TryGetTagByName(tag.Type, tag.Value, out DomainTag? existingTag);
        if (existingTag is null)
        {
            return DomainTag.Create(user, tag.Type, tag.Value);
        }
        else
        {
            return existingTag;
        }
    }
}