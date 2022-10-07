namespace Inklio.Api.Application.Commands;

public class AskCreateCommandValidator : AbstractValidator<AskCreateCommand>
{
    public AskCreateCommandValidator()
    {
        RuleFor(c => c.Body).NotEmpty().Length(1, 4000).WithMessage("Ask body was too long.");
        RuleFor(c => c.Title).NotEmpty().Length(1, 256).WithMessage("Ask title length was too long.");
        // RuleFor(c => c.Tags).NotEmpty().WithMessage("Ask must have at least one tag."); // TODO re-enable. Disabled because Invoke-webrequest doesn't work proprely and testing is hard
        RuleFor(c => c.Tags).Must(t => t.Count() <= Inklio.Api.Domain.Ask.MaxTagCount).WithMessage($"Ask must have fewer than {Inklio.Api.Domain.Ask.MaxTagCount} tags.");
    }
}