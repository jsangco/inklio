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

    [Authorize(Roles = "Moderator, Administrator")]
    [EnableQuery()]
    [HttpGet("all")]
    public IQueryable<Inklio.Api.Application.Commands.Ask> GetAllAsks()
    {
        var userId = this.User.UserIdOrDefault();
        var asks = this.mapper.ProjectTo<Inklio.Api.Application.Commands.Ask>(this.askRepository.GetAllAsks(userId));
        return asks.Take(1);
    }

    [Authorize(Roles = "Moderator, Administrator")]
    [HttpGet("all/{askId:int}")]
    public async Task<Inklio.Api.Application.Commands.Ask> GetAnyAskById(
        int askId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAnyAskByIdAsync(askId, cancellationToken);
        var askDto = this.mapper.Map<Inklio.Api.Application.Commands.Ask>(ask);
        return askDto ?? throw new InvalidOperationException("Could not map Ask DTO");
    }

    [EnableQuery()]
    [HttpGet("{askId:int}")]
    public async Task<Inklio.Api.Application.Commands.Ask> GetAskById(
        int askId,
        CancellationToken cancellationToken)
    {
        var userId = this.User.UserIdOrDefault();
        var ask = await this.askRepository.GetAskByIdAsync(askId, userId, cancellationToken);
        var askDto = this.mapper.Map<Inklio.Api.Application.Commands.Ask>(ask);
        return askDto ?? throw new InvalidOperationException("Could not map Ask DTO");
    }

    [EnableQuery()]
    [HttpGet("{askId:int}/comments")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.AskComment>> GetComments(
        int askId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var comments = this.mapper.ProjectTo<Inklio.Api.Application.Commands.AskComment>(ask.Comments.AsQueryable());
        return comments;
    }

    [EnableQuery()]
    [HttpGet("{askId:int}/deliveries")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.Delivery>> GetDeliveries(
        int askId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var deliveries = this.mapper.ProjectTo<Inklio.Api.Application.Commands.Delivery>(ask.Deliveries.AsQueryable());
        return deliveries;
    }

    [EnableQuery()]
    [HttpGet("{askId:int}/deliveries/{deliveryId:int}")]
    public async Task<Inklio.Api.Application.Commands.Delivery> GetDeliveryById(
        int askId,
        int deliveryId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var delivery = ask.Deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(404, $"Delivery {deliveryId:int} could not be found");
        }

        var deliveryDto = this.mapper.Map<Inklio.Api.Application.Commands.Delivery>(delivery);
        return deliveryDto ?? throw new InvalidOperationException("Could not map Delivery DTO");
    }

    [EnableQuery()]
    [HttpGet("{askId:int}/deliveries/{deliveryId:int}/comments")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.DeliveryComment>> GetDeliveryComments(
        int askId,
        int deliveryId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var delivery = ask.Deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(404, $"Delivery {deliveryId:int} could not be found");
        }

        var comments = this.mapper.ProjectTo<Inklio.Api.Application.Commands.DeliveryComment>(delivery.Comments.AsQueryable());

        return comments;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAsk(
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
    [HttpPost("{askId:int}/comments")]
    public async Task CreateAskComment(
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
    [HttpPost("{askId:int}/tags")]
    public async Task CreateAskTag(
        int askId,
        [FromBody] AskTagCreateCommand tagCommand,
        CancellationToken cancellationToken)
    {
        tagCommand.AskId = askId;
        tagCommand.UserId = this.User.UserId();

        this.logger.LogInformation("----- Sending command: {CommandName}", tagCommand.GetGenericTypeName());
        await this.mediator.Send(tagCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId:int}/upvote")]
    public async Task CreateAskUpvote(int askId, CancellationToken cancellationToken)
    {
        var upvoteCreateCommand = new UpvoteCreateCommand(askId, null, null, this.User.UserId());
        this.logger.LogInformation("----- Sending command: {CommandName}", upvoteCreateCommand.GetGenericTypeName());
        await this.mediator.Send(upvoteCreateCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId:int}/comments/{commentId:int}/upvote")]
    public async Task CreateAskCommentUpvote(int askId, int commentId, CancellationToken cancellationToken)
    {
        var upvoteCreateCommand = new UpvoteCreateCommand(askId, commentId, null, this.User.UserId());
        this.logger.LogInformation("----- Sending command: {CommandName}", upvoteCreateCommand.GetGenericTypeName());
        await this.mediator.Send(upvoteCreateCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId:int}/deliveries")]
    public async Task<IActionResult> CreateDelivery(
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
    [HttpPost("{askId:int}/deliveries/{deliveryId:int}/comments")]
    public async Task CreateDeliveryComment(
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
    [HttpPost("{askId:int}/deliveries/{deliveryId:int}/tags")]
    public async Task CreateDeliveryTag(
        int askId,
        int deliveryId,
        [FromBody] DeliveryTagCreateCommand tagCommand,
        CancellationToken cancellationToken)
    {
        tagCommand.AskId = askId;
        tagCommand.DeliveryId = deliveryId;
        tagCommand.UserId = this.User.UserId();

        this.logger.LogInformation("----- Sending command: ask {CommandName}", tagCommand.GetGenericTypeName());
        await this.mediator.Send(tagCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId:int}/deliveries/{deliveryId:int}/upvote")]
    public async Task CreateDeliveryUpvote(int askId, int deliveryId, CancellationToken cancellationToken)
    {
        var upvoteCreateCommand = new UpvoteCreateCommand(askId, null, deliveryId, this.User.UserId());
        this.logger.LogInformation("----- Sending command: delivery {CommandName}", upvoteCreateCommand.GetGenericTypeName());
        await this.mediator.Send(upvoteCreateCommand, cancellationToken);
    }

    [Authorize]
    [HttpPost("{askId:int}/deliveries/{deliveryId:int}/comments/{commentId:int}/upvote")]
    public async Task CreateDeliveryCommentUpvote(int askId, int deliveryId, int commentId, CancellationToken cancellationToken)
    {
        var upvoteCreateCommand = new UpvoteCreateCommand(askId, commentId, deliveryId, this.User.UserId());
        this.logger.LogInformation("----- Sending command: delivery comment {CommandName}", upvoteCreateCommand.GetGenericTypeName());
        await this.mediator.Send(upvoteCreateCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId:int}")]
    public async Task DeleteAsk(int askId, [FromBody] DeletionCommand deletionCommand, CancellationToken cancellationToken)
    {
        deletionCommand.AskId = askId;
        deletionCommand.EditedById = this.User.UserId();
        deletionCommand.DeletionType = this.User.IsModerator() ? deletionCommand.DeletionType : DeletionType.CreatorDeleted;
        deletionCommand.UserMessage = this.User.IsModerator() ? deletionCommand.UserMessage : ""; // Ignored for self-deletion
        deletionCommand.InternalComment = this.User.IsModerator() ? deletionCommand.InternalComment : ""; // Ignored for self-deletion
        deletionCommand.IsModeratorDeletion = this.User.IsModerator();

        this.logger.LogInformation("----- Sending command: ask {CommandName}", deletionCommand.GetGenericTypeName());
        await this.mediator.Send(deletionCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId:int}/upvote")]
    public async Task DeleteAskUpvoteAsync(int askId, CancellationToken cancellationToken)
    {
        var upvoteDeleteCommand = new UpvoteDeleteCommand(askId, null, null, this.User.UserId());
        this.logger.LogInformation("----- Sending command: ask {CommandName}", upvoteDeleteCommand.GetGenericTypeName());
        await this.mediator.Send(upvoteDeleteCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId:int}/comments/{commentId:int}")]
    public async Task DeleteAskCommentAsync(int askId, int commentId, [FromBody] DeletionCommand deletionCommand, CancellationToken cancellationToken)
    {
        deletionCommand.EditedById = this.User.UserId();
        deletionCommand.AskId = askId;
        deletionCommand.CommentId = commentId;
        deletionCommand.DeletionType = this.User.IsModerator() ? deletionCommand.DeletionType : DeletionType.CreatorDeleted;
        deletionCommand.UserMessage = this.User.IsModerator() ? deletionCommand.UserMessage : ""; // Ignored for self-deletion
        deletionCommand.InternalComment = this.User.IsModerator() ? deletionCommand.InternalComment : ""; // Ignored for self-deletion

        this.logger.LogInformation("----- Sending command: ask comment {CommandName}", deletionCommand.GetGenericTypeName());
        await this.mediator.Send(deletionCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId:int}/comments/{commentId:int}/upvote")]
    public async Task DeleteCommentUpvoteAsync(int askId, int commentId, CancellationToken cancellationToken)
    {
        var upvoteDeleteCommand = new UpvoteDeleteCommand(askId, commentId, null, this.User.UserId());
        this.logger.LogInformation("----- Sending command: ask comment {CommandName}", upvoteDeleteCommand.GetGenericTypeName());
        await this.mediator.Send(upvoteDeleteCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId:int}/deliveries/{deliveryId:int}")]
    public async Task DeleteDeliveryeryAsync(int askId, int deliveryId, [FromBody] DeletionCommand deletionCommand, CancellationToken cancellationToken)
    {
        deletionCommand.EditedById = this.User.UserId();
        deletionCommand.AskId = askId;
        deletionCommand.DeliveryId = deliveryId;
        deletionCommand.DeletionType = this.User.IsModerator() ? deletionCommand.DeletionType : DeletionType.CreatorDeleted;
        deletionCommand.UserMessage = this.User.IsModerator() ? deletionCommand.UserMessage : ""; // Ignored for self-deletion
        deletionCommand.InternalComment = this.User.IsModerator() ? deletionCommand.InternalComment : ""; // Ignored for self-deletion
        deletionCommand.IsModeratorDeletion = this.User.IsModerator();

        this.logger.LogInformation("----- Sending command: delivery {CommandName}", deletionCommand.GetGenericTypeName());
        await this.mediator.Send(deletionCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId:int}/deliveries/{deliveryId:int}/upvote")]
    public async Task DeleteDeliveryUpvoteAsync(int askId, int deliveryId, CancellationToken cancellationToken)
    {
        var upvoteDeleteCommand = new UpvoteDeleteCommand(askId, null, deliveryId, this.User.UserId());
        this.logger.LogInformation("----- Sending command: delivery {CommandName}", upvoteDeleteCommand.GetGenericTypeName());
        await this.mediator.Send(upvoteDeleteCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId:int}/deliveries/{deliveryId:int}/comments/{commentId:int}")]
    public async Task DeleteDeliveryCommentAsync(int askId, int deliveryId, int commentId, [FromBody] DeletionCommand deletionCommand, CancellationToken cancellationToken)
    {
        deletionCommand.EditedById = this.User.UserId();
        deletionCommand.AskId = askId;
        deletionCommand.DeliveryId = deliveryId;
        deletionCommand.CommentId = commentId;
        deletionCommand.DeletionType = this.User.IsModerator() ? deletionCommand.DeletionType : DeletionType.CreatorDeleted;
        deletionCommand.UserMessage = this.User.IsModerator() ? deletionCommand.UserMessage : ""; // Ignored for self-deletion
        deletionCommand.InternalComment = this.User.IsModerator() ? deletionCommand.InternalComment : ""; // Ignored for self-deletion
        deletionCommand.IsModeratorDeletion = this.User.IsModerator();

        this.logger.LogInformation("----- Sending command: delivery comment {CommandName}", deletionCommand.GetGenericTypeName());
        await this.mediator.Send(deletionCommand, cancellationToken);
    }

    [Authorize]
    [HttpDelete("{askId:int}/deliveries/{deliveryId:int}/comments/{commentId:int}/upvote")]
    public async Task DeleteDeliveryCommentUpvoteAsync(int askId, int commentId, int deliveryId, CancellationToken cancellationToken)
    {
        var upvoteDeleteCommand = new UpvoteDeleteCommand(askId, commentId, deliveryId, this.User.UserId());
        this.logger.LogInformation("----- Sending command: delivery comment {CommandName}", upvoteDeleteCommand.GetGenericTypeName());
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
