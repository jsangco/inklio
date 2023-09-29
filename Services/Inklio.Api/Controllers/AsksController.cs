using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using AutoMapper;
using EFCore.NamingConventions.Internal;
using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Inklio.Api.Controllers;

[ApiController]
[Route("v1/asks")]
public class AsksController : ODataController
{
    private readonly ILogger<AsksController> logger;
    private readonly IAskRepository askRepository;
    private readonly IMediator mediator;
    private readonly IWebHostEnvironment hostEnvironment;
    private readonly IMapper mapper;

    public AsksController(
        ILogger<AsksController> logger,
        IAskRepository askRepository,
        IMediator mediator,
        IMapper mapper,
        IWebHostEnvironment hostEnvironment)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        this.mapper = mapper ?? throw new ArgumentException(nameof(mapper));
    }

    [EnableQuery()]
    [HttpGet]
    public IQueryable<Inklio.Api.Application.Commands.Ask> GetAsks()
    {
        var userId = this.User.UserIdOrDefault();
        var asks = this.mapper.ProjectTo<Inklio.Api.Application.Commands.Ask>(this.askRepository.GetAsks(userId));
        return asks;
    }

    [EnableQuery()]
    [HttpGet("{askId}")]
    public async Task<Inklio.Api.Application.Commands.Ask> GetAskById(
        int askId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var askDto = this.mapper.Map<Inklio.Api.Application.Commands.Ask>(ask);
        return askDto ?? throw new InvalidOperationException("Could not map Ask DTO");
    }

    [EnableQuery()]
    [HttpGet("{askId}/comments")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.AskComment>> GetComments(
        int askId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var comments = this.mapper.ProjectTo<Inklio.Api.Application.Commands.AskComment>(ask.Comments.AsQueryable());
        return comments;
    }

    [EnableQuery()]
    [HttpGet("{askId}/deliveries")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.Delivery>> GetDeliveries(
        int askId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var deliveries = this.mapper.ProjectTo<Inklio.Api.Application.Commands.Delivery>(ask.Deliveries.AsQueryable());
        return deliveries;
    }

    [EnableQuery()]
    [HttpGet("{askId}/deliveries/{deliveryId}")]
    public async Task<Inklio.Api.Application.Commands.Delivery> GetDeliveryById(
        int askId,
        int deliveryId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var delivery = ask.Deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(404, $"Delivery {deliveryId} could not be found");
        }

        var deliveryDto = this.mapper.Map<Inklio.Api.Application.Commands.Delivery>(delivery);
        return deliveryDto ?? throw new InvalidOperationException("Could not map Delivery DTO");
    }

    [EnableQuery()]
    [HttpGet("{askId}/deliveries/{deliveryId}/comments")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.DeliveryComment>> GetDeliveryComments(
        int askId,
        int deliveryId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var delivery = ask.Deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(404, $"Delivery {deliveryId} could not be found");
        }

        var comments = this.mapper.ProjectTo<Inklio.Api.Application.Commands.DeliveryComment>(delivery.Comments.AsQueryable());

        return comments;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddAsk(
        [FromForm] AskCreateForm askCreateForm,
        CancellationToken cancellationToken)
    {
        (var askCreateCommand, var problemDetails) = this.DeserializeAskCreate(askCreateForm);
        if (problemDetails is not null)
        {
            return this.BadRequest(problemDetails);
        }

        askCreateCommand.UserId = this.User.UserId();

        this.logger.LogInformation("----- Sending command: {CommandName}", askCreateCommand.GetGenericTypeName());
        await this.mediator.Send(askCreateCommand, cancellationToken);

        return this.Accepted();
    }

    [Authorize]
    [HttpPost("{askId}/comments")]
    public async Task AddAskComment(
         int askId,
        [FromBody] AskCommentCreateCommand commentCreateCommand,
        CancellationToken cancellationToken)
    {
        commentCreateCommand.AskId = askId;
        commentCreateCommand.UserId = this.User.UserId();

        this.logger.LogInformation("----- Sending command: {CommandName}", commentCreateCommand.GetGenericTypeName());
        await this.mediator.Send(commentCreateCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId}/tags")]
    public async Task AddAskTag(
        int askId,
        [FromBody] AskTagAddCommand tagCommand,
        CancellationToken cancellationToken)
    {
        tagCommand.AskId = askId;
        tagCommand.UserId = this.User.UserId();

        this.logger.LogInformation("----- Sending command: {CommandName}", tagCommand.GetGenericTypeName());
        await this.mediator.Send(tagCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId}/upvote")]
    public async Task AddAskUpvote(int askId, CancellationToken cancellationToken)
    {
        var upvoteCreateCommand = new UpvoteCreateCommand(askId, null, this.User.UserId());
        this.logger.LogInformation("----- Sending command: {CommandName}", upvoteCreateCommand.GetGenericTypeName());
        await this.mediator.Send(upvoteCreateCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId}/deliveries")]
    public async Task<IActionResult> AddDelivery(
        int askId,
        [FromForm] DeliveryCreateForm deliveryCreateForm,
        CancellationToken cancellationToken)
    {
        (var deliveryCreateCommand, var problemDetails) = this.DeserializeDeliveryCreate(deliveryCreateForm);
        if (problemDetails is not null)
        {
            return this.BadRequest(problemDetails);
        }

        deliveryCreateCommand.AskId = askId;
        deliveryCreateCommand.UserId = this.User.UserId();

        this.logger.LogInformation("----- Sending command: {CommandName}", deliveryCreateCommand.GetGenericTypeName());
        await this.mediator.Send(deliveryCreateCommand, cancellationToken);
        return this.Accepted();
    }

    [Authorize]
    [HttpPost("{askId}/deliveries/{deliveryId}/comments")]
    public async Task AddDeliveryComment(
        int askId,
        int deliveryId,
        [FromBody] DeliveryCommentCreateCommand commentCreateCommand,
        CancellationToken cancellationToken)
    {
        commentCreateCommand.AskId = askId;
        commentCreateCommand.DeliveryId = deliveryId;
        commentCreateCommand.UserId = this.User.UserId();

        this.logger.LogInformation("----- Sending command: {CommandName}", commentCreateCommand.GetGenericTypeName());
        await this.mediator.Send(commentCreateCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId}/deliveries/{deliveryId}/tags")]
    public async Task AddDeliveryTag(
        int askId,
        int deliveryId,
        [FromBody] DeliveryTagAddCommand tagCommand,
        CancellationToken cancellationToken)
    {
        tagCommand.AskId = askId;
        tagCommand.DeliveryId = deliveryId;
        tagCommand.UserId = this.User.UserId();

        this.logger.LogInformation("----- Sending command: ask {CommandName}", tagCommand.GetGenericTypeName());
        await this.mediator.Send(tagCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId}/deliveries/{deliveryId}/upvote")]
    public async Task AddDeliveryUpvote(int askId, int deliveryId, CancellationToken cancellationToken)
    {
        var upvoteCreateCommand = new UpvoteCreateCommand(askId, deliveryId, this.User.UserId());
        this.logger.LogInformation("----- Sending command: delivery {CommandName}", upvoteCreateCommand.GetGenericTypeName());
        await this.mediator.Send(upvoteCreateCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId}/upvote")]
    public async Task DeleteAskUpvoteAsync(int askId, CancellationToken cancellationToken)
    {
        var upvoteDeleteCommand = new UpvoteDeleteCommand(askId, null, this.User.UserId());
        await this.mediator.Send(upvoteDeleteCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId}/deliveries/{deliveryId}/upvote")]
    public async Task DeleteDeliveryUpvoteAsync(int askId, int deliveryId, CancellationToken cancellationToken)
    {
        var upvoteDeleteCommand = new UpvoteDeleteCommand(askId, deliveryId, this.User.UserId());
        await this.mediator.Send(upvoteDeleteCommand, cancellationToken);
    }

    /// <summary>
    /// Deserialize the Ask form data into a AskCreateCommand.
    /// </summary>
    /// <param name="askCreateForm">The ask create form</param>
    /// <returns>An AskCreate command and a problem details collection if the request was invalid.</returns>
    private (AskCreateCommand, ValidationProblemDetails?) DeserializeAskCreate(AskCreateForm askCreateForm)
    {
        // FIXME: This code dulicates the below DeliveryDeliveryCreate function and does a bunch of awkward manual validation.
        try
        {
            var ask = JsonSerializer.Deserialize<AskCreateCommand>(askCreateForm.Ask);
            if (string.IsNullOrWhiteSpace(askCreateForm.Ask) || ask is null)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = this.Request.Path,
                    Status = 400,
                    Detail = "Invalid Ask.",
                };
                problemDetails.Errors.Add(new KeyValuePair<string, string[]>("ask", new string[] { "Missing or invalid Ask." }));
                return (new AskCreateCommand(), problemDetails);
            }

            // Validate the Ask
            var validationContext = new ValidationContext(ask);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(ask, validationContext, validationResults);
            bool tagsValid = ask.Tags.Aggregate(true, (acc, t) =>
            {
                var vc = new ValidationContext(t);
                return acc && Validator.TryValidateObject(t, vc, null);
            });
            if (isValid && tagsValid)
            {
                if (askCreateForm.Images is not null)
                {
                    ask.Images = askCreateForm.Images;
                }
                return (ask, null);
            }
            else
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = this.Request.Path,
                    Status = 400,
                    Detail = "Invalid Ask.",
                };
                SnakeCaseNameRewriter snakeCase = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);
                var errors = validationResults.Select(i =>
                    new KeyValuePair<string, string[]>(
                        snakeCase.RewriteName(i.MemberNames.FirstOrDefault() ?? "property"),
                        new string[] { i?.ErrorMessage ?? "Error" }));
                foreach (var error in errors)
                {
                    problemDetails.Errors.Add(error);
                }
                if (tagsValid == false)
                {
                    problemDetails.Errors.Add(new KeyValuePair<string, string[]>("tags", new string[] { "Invalid tag." }));
                }

                return (new AskCreateCommand(), problemDetails);
            }
        }
        catch (JsonException)
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = this.Request.Path,
                Status = 400,
                Detail = "Invalid Ask.",
            };
            problemDetails.Errors.Add(new KeyValuePair<string, string[]>("ask", new string[] { "Missing or invalid Ask." }));
            return (new AskCreateCommand(), problemDetails);
        }
    }

    /// <summary>
    /// Deserialize the Ask form data into a AskCreateCommand.
    /// </summary>
    /// <param name="askCreateForm">The ask create form</param>
    /// <returns>An AskCreate command and a problem details collection if the request was invalid.</returns>
    private (DeliveryCreateCommand, ValidationProblemDetails?) DeserializeDeliveryCreate(DeliveryCreateForm deliveryCreateForm)
    {
        // FIXME: This code dulicates the above DeliveryAskCreate function and does a bunch of awkward manual validation.

        try
        {
            var delivery = JsonSerializer.Deserialize<DeliveryCreateCommand>(deliveryCreateForm.Delivery);
            if (string.IsNullOrWhiteSpace(deliveryCreateForm.Delivery) || delivery is null)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = this.Request.Path,
                    Status = 400,
                    Detail = "Invalid Delivery.",
                };
                problemDetails.Errors.Add(new KeyValuePair<string, string[]>("delivery", new string[] { "Missing or invalid Delivery." }));
                return (new DeliveryCreateCommand(), problemDetails);
            }
            delivery.Images = deliveryCreateForm.Images;

            // Validate the Delivery
            var validationContext = new ValidationContext(delivery);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(delivery, validationContext, validationResults);
            bool tagsValid = delivery.Tags.Aggregate(true, (acc, t) =>
            {
                var vc = new ValidationContext(t);
                return acc && Validator.TryValidateObject(t, vc, null);
            });
            if (isValid && tagsValid)
            {
                return (delivery, null);
            }
            else
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = this.Request.Path,
                    Status = 400,
                    Detail = "Invalid Delivery.",
                };
                SnakeCaseNameRewriter snakeCase = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);
                var errors = validationResults.Select(i =>
                    new KeyValuePair<string, string[]>(
                        snakeCase.RewriteName(i.MemberNames.FirstOrDefault() ?? "property"),
                        new string[] { i?.ErrorMessage ?? "Error" }));
                foreach (var error in errors)
                {
                    problemDetails.Errors.Add(error);
                }
                if (tagsValid == false)
                {
                    problemDetails.Errors.Add(new KeyValuePair<string, string[]>("tags", new string[] { "Invalid tag." }));
                }

                return (new DeliveryCreateCommand(), problemDetails);
            }
        }
        catch (JsonException)
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = this.Request.Path,
                Status = 400,
                Detail = "Invalid Delivery.",
            };
            problemDetails.Errors.Add(new KeyValuePair<string, string[]>("delivery", new string[] { "Missing or invalid Delivery." }));
            return (new DeliveryCreateCommand(), problemDetails);
        }
    }
}
