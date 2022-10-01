namespace Inklio.Api.Application.Commands;

public class AskCreateCommandValidator : AbstractValidator<AskCreateCommand>
{
    public AskCreateCommandValidator()
    {
        RuleFor(c => c.Body).NotEmpty().Length(1, 4000);
        RuleFor(c => c.Title).NotEmpty().Length(1, 256);
        RuleFor(c => c.Tags).NotEmpty().WithMessage("Must have at least one tag.");
        RuleFor(c => c.Tags).Must(t => t.Count() <= Inklio.Api.Domain.Ask.MaxTagCount).WithMessage($"Must have fewer than {Inklio.Api.Domain.Ask.MaxTagCount} tags.");
    }
}