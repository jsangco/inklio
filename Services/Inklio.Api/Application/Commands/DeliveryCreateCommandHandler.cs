using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using DomainAsk = Inklio.Api.Domain.Ask;
using DomainTag = Inklio.Api.Domain.Tag;
using DomainUser = Inklio.Api.Domain.User;

public class DeliveryCreateCommandHandler : IRequestHandler<DeliveryCreateCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;
    private readonly ITagRepository tagRepository;

    /// <summary>
    /// Initializes an instance of a <see cref="DeliveryCreateCommandHandler"/> object.
    /// </summary>
    /// <param name="askRepository">A repository for <see cref="DomainAsk"/> objects</param>
    /// <param name="userRepository">A repository for <see cref="DomainUser"/> objects</param>
    /// <param name="tagRepository">A repository for <see cref="DomainTag"/> objects</param>
    /// <exception cref="ArgumentNullException"></exception>
    public DeliveryCreateCommandHandler(IAskRepository askRepository, IUserRepository userRepository, ITagRepository tagRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
    }

    /// <inheritdoc/>
    public async Task<bool> Handle(DeliveryCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userRepository.GetByIdAsync(request.UserId, cancellationToken);
        var ask = await this.askRepository.GetAskByIdAsync(request.AskId, cancellationToken);

        ask.AddDelivery(request.Body, user, request.IsNsfl, request.IsNswf, request.Title);

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}