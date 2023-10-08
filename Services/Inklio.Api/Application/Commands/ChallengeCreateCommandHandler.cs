using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using MediatR;

/// <summary>
/// Handler for creating ask comments
/// </summary>
public class ChallengeCreateCommandHandler : IRequestHandler<ChallengeCreateCommand, bool>
{
    private readonly IAskRepository askRepository;
    private readonly IUserRepository userRepository;

    public ChallengeCreateCommandHandler(IAskRepository askRepository, IUserRepository userRepository)
    {
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public async Task<bool> Handle(ChallengeCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await this.userRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var ask = await this.askRepository.GetAskByIdAsync(request.AskId, cancellationToken);

        ask.AddChallenge(request.ChallengeType, DateTime.UtcNow, user, request.StartAtUtc, request.EndAtUtc);

        await this.askRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}