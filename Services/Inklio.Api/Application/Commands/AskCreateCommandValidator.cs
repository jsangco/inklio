namespace Inklio.Api.Application.Commands;

public class AskCreateCommandValidator : AbstractValidator<AskCreateCommand>
{
    public AskCreateCommandValidator()
    {
        RuleFor(c => c.Body).NotEmpty().Length(1, 4000);
        RuleFor(c => c.Title).NotEmpty().Length(1, 256);
    }
}