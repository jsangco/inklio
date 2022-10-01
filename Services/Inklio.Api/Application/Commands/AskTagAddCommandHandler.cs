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
    private readonly ITagRepository tagRepository;

    /// <summary>
    /// Initialize a new instance of an <see cref="AskTagAddCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <param name="tagRepository">A repository for tag objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AskTagAddCommandHandler(IAskRepository askRepository, IUserRepository userRepository, ITagRepository tagRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
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
        var ask = await this.askRepository.GetByIdAsync(request.AskId, cancellationToken);

        (DomainTag tag, bool isNewTag) = await GetOrCreateTagAsync(request.Tag, user, cancellationToken);

        try
        {
            ask.AddTag(request.TagDeliveries, user, tag);
        }
        catch (InklioDomainException e)
        {
            // If the Domain logic decides we cannot add the tag and the tag was newly created,
            // we have to delete the newly tag created tag. However, because the Ask repo and the
            // Tag repo are different AggregateRoots the deletion is done as a compensary action.
            if (isNewTag)
            {
                try
                {
                    this.tagRepository.Delete(request.Tag.Type, request.Tag.Value);
                }
                catch (InklioDomainException)
                {
                    throw new InvalidOperationException("Failed compensary transaction when trying to remove tag");
                }
            }

            throw e; // Send the domain exception back to the client
        }

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Gets a tag from the repository. If a tag does not exist, it is added to the repository.
    /// </summary>
    /// <param name="tag">The tag to get.</param>
    /// <param name="user">The user creating getting or creating the tags</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A collection of all relevant Tags that were retrieved from the repository.</returns>
    private async Task<(DomainTag, bool)> GetOrCreateTagAsync(CommandTag tag, User user, CancellationToken cancellationToken)
    {
        this.tagRepository.TryGetByName(tag.Type, tag.Value, out DomainTag? existingTag);
        if (existingTag is null)
        {
            var newTag = DomainTag.Create(user, tag.Type, tag.Value);
            await this.tagRepository.AddAsync(newTag, cancellationToken);
            await this.tagRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return (newTag, true);
        }
        else
        {
            return (existingTag, false);
        }
    }
}