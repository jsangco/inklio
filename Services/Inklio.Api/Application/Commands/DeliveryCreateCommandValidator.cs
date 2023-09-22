namespace Inklio.Api.Application.Commands;

public class DeliveryCreateCommandValidator : AbstractValidator<DeliveryCreateCommand>
{
    public DeliveryCreateCommandValidator()
    {
        When(d => string.IsNullOrEmpty(d.Body), () =>
        {
            RuleFor(d => d.Images).NotEmpty().WithMessage("Deliveries must contain an image or written text.");
        });
    }
}