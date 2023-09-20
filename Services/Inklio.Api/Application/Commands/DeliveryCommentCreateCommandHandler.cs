using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using MediatR;

public class DeliveryCommentCreateCommandHandler : IRequestHandler<DeliveryCommentCreateCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;
    private readonly ITagRepository tagRepository;

    public DeliveryCommentCreateCommandHandler(IAskRepository askRepository, IUserRepository userRepository, ITagRepository tagRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
    }

    public async Task<bool> Handle(DeliveryCommentCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var ask = await this.askRepository.GetAskByIdAsync(request.AskId, cancellationToken);
        ask.AddDeliveryComment(request.Body, request.DeliveryId, user);

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}