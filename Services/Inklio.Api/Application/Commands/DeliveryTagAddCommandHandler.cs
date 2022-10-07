using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using CommandTag = Inklio.Api.Application.Commands.Tag;
using DomainComment = Inklio.Api.Domain.Comment;
using DomainTag = Inklio.Api.Domain.Tag;

/// <summary>
/// A handler that adds a Tag to a Delivery. 
/// </summary>
public class DeliveryTagAddCommandHandler : IRequestHandler<DeliveryTagAddCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;

    /// <summary>
    /// Initialize a new instance of an <see cref="DeliveryTagAddCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <param name="tagRepository">A repository for tag objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public DeliveryTagAddCommandHandler(IAskRepository askRepository, IUserRepository userRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Add a tag to an Delivery.
    /// </summary>
    /// <param name="request">The command to add the tag to the Delivery.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A flag indicating whether the tag addition was a success.</returns>
    public async Task<bool> Handle(DeliveryTagAddCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userRepository.GetByIdAsync(request.UserId, cancellationToken);
        var ask = await this.askRepository.GetAskByIdAsync(request.AskId, cancellationToken);

        DomainTag tag = this.GetOrCreateTag(request.Tag, user);

        var delivery = ask.Deliveries.FirstOrDefault(d => d.Id == request.DeliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(400, $"The specified Delivery {request.DeliveryId} does not exist within the specified Ask {request.AskId}");
        }
        delivery.AddTag(user, tag);

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