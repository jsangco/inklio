using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using MediatR;
using CommandComment = Inklio.Api.Application.Commands.Comment;
using DomainComment = Inklio.Api.Domain.Comment;
using DomainTag = Inklio.Api.Domain.Tag;

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
        var user = await this.userRepository.GetByIdAsync(request.UserId, cancellationToken);
        var ask = await this.askRepository.GetByIdAsync(request.AskId, cancellationToken);

        ask.AddComment(request.Body, user);

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}