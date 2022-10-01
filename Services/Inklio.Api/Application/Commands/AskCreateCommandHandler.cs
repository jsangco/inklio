using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using MediatR;
using CommandAsk = Inklio.Api.Application.Commands.Ask;
using CommandTag = Inklio.Api.Application.Commands.Tag;
using DomainAsk = Inklio.Api.Domain.Ask;
using DomainTag = Inklio.Api.Domain.Tag;

public class AskCreateCommandHandler : IRequestHandler<AskCreateCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;
    private readonly ITagRepository tagRepository;

    /// <summary>
    /// Initializes an instance of a new <see cref="AskCreateCommandHandler"/>
    /// </summary>
    /// <param name="askRepository">A repository for ask objects</param>
    /// <param name="userRepository">A repository for user objects</param>
    /// <param name="tagRepository">A repository for tag objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public AskCreateCommandHandler(IAskRepository askRepository, IUserRepository userRepository, ITagRepository tagRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
    }

    /// <summary>
    /// Creates a new Ask.
    /// </summary>
    /// <param name="request">The command to create the Ask.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A flag indicating whether the Ask creation was a success.</returns>
    public async Task<bool> Handle(AskCreateCommand request, CancellationToken cancellationToken)
    {
        // Get the user creating the tag
        User user = await this.userRepository.GetByIdAsync(request.UserId, cancellationToken);

        // Create the ask
        DomainAsk ask = DomainAsk.Create(request.Body, user, request.IsNsfl, request.IsNswf, request.Title);

        // Get and add tags to the new Ask
        IEnumerable<DomainTag> tags = await this.GetOrCreateTagsAsync(request.Tags, user, cancellationToken);
        foreach (var tag in tags)
        {
            ask.AddTag(false, user, tag);
        }

        // Add the ask in the repository
        await this.askRepository.AddAsync(ask, cancellationToken);

        // Save changes
        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Gets all tags from the tag repository. If a tag does not exist, it is added to the repository.
    /// </summary>
    /// <param name="tags">The tags to get.</param>
    /// <param name="user">The user creating getting or creating the tags</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A collection of all relevant Tags that were retrieved from the repository.</returns>
    private async Task<IEnumerable<DomainTag>> GetOrCreateTagsAsync(IEnumerable<CommandTag> tags, User user, CancellationToken cancellationToken)
    {
        // Get all existing tags
        var existingTags = new List<DomainTag>();
        foreach (var askTag in tags)
        {
            this.tagRepository.TryGetByName(askTag.Type, askTag.Value, out DomainTag? tag);
            if (tag is not null)
            {
                existingTags.Add(tag);
            }
        }
        
        // Add any tags that don't exist to the repository.
        var newTags = tags.Select(t => t.ToString()).Except(existingTags.Select(t => t.ToString()));
        foreach (var newTag in newTags)
        {
            var tagSplit = newTag.Split(':');
            var tag = DomainTag.Create(user, tagSplit[0], tagSplit[1]);
            await this.tagRepository.AddAsync(tag, cancellationToken);

            // Add to the list of existing tags to be returned later
            existingTags.Add(tag);
        }

        await this.tagRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return existingTags;
    }
}