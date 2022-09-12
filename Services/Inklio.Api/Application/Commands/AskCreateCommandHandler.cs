using Inklio.Api.Application.Commands;
using MediatR;

public class AskCreateCommandHandler : IRequestHandler<AskCreateCommand, Ask>
{
    public Task<Ask> Handle(AskCreateCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Ask());
    }
}