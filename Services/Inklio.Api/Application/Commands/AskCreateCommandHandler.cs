using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using MediatR;
using CommandAsk = Inklio.Api.Application.Commands.Ask;
using DomainAsk = Inklio.Api.Domain.Ask;
using DomainTag = Inklio.Api.Domain.Tag;

public class AskCreateCommandHandler : IRequestHandler<AskCreateCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;
    private readonly ITagRepository tagRepository;

    public AskCreateCommandHandler(IAskRepository askRepository, IUserRepository userRepository, ITagRepository tagRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
    }

    public async Task<bool> Handle(AskCreateCommand request, CancellationToken cancellationToken)
    {
        // TODO - Future Validation
        // 1. User has not created any asks within the last X hours
        // 2. User has the ability to create a new ask
        // 3. Validate user can create the specified tag
        // 4. Validate the user can use the specified tag

        // Get the user creating the tag
        var user = await this.userRepository.GetByIdAsync(request.UserId, cancellationToken);

        // Create the ask
        var ask = new DomainAsk(request.Body, user, request.IsNsfl, request.IsNswf, request.Title);

        // Add related tags to the ask
        foreach (var askTag in request.Tags)
        {
            var tag = this.tagRepository.GetOrCreate(askTag.Type, askTag.Value);
            ask.AddTag(false, user, tag);
        }

        // Add and save the ask in the repository
        await this.askRepository.AddAsync(ask, cancellationToken);
        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}