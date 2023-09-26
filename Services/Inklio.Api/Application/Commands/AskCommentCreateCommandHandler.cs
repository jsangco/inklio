using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using MediatR;

/// <summary>
/// Handler for creating ask comments
/// </summary>
public class AskCommentCreateCommandHandler : IRequestHandler<AskCommentCreateCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;
    private readonly ITagRepository tagRepository;

    public AskCommentCreateCommandHandler(IAskRepository askRepository, IUserRepository userRepository, ITagRepository tagRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
    }

    public async Task<bool> Handle(AskCommentCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var ask = await this.askRepository.GetAskByIdAsync(request.AskId, cancellationToken);

        ask.AddComment(request.Body, user);

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}