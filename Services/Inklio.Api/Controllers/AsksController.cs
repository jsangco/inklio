using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Inklio.Api.Infrastructure.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Inklio.Api.Controllers;

public class AsksController : ODataController
{
    private readonly ILogger<AsksController> logger;
    private readonly IAskRepository askRepository;
    private readonly IMediator mediator;
    private readonly InklioContext context;
    private readonly IWebHostEnvironment hostEnvironment;
    private readonly IMapper projector = new MapperConfiguration(cfg =>
    {
        cfg.CreateProjection<Inklio.Api.Domain.Ask, Inklio.Api.Application.Commands.Ask>();
        cfg.CreateProjection<Inklio.Api.Domain.AskComment, Inklio.Api.Application.Commands.AskComment>();
        cfg.CreateProjection<Inklio.Api.Domain.AskImage, Inklio.Api.Application.Commands.AskImage>();
        cfg.CreateProjection<Inklio.Api.Domain.Comment, Inklio.Api.Application.Commands.Comment>();
        cfg.CreateProjection<Inklio.Api.Domain.Delivery, Inklio.Api.Application.Commands.Delivery>();
        cfg.CreateProjection<Inklio.Api.Domain.DeliveryComment, Inklio.Api.Application.Commands.DeliveryComment>();
        cfg.CreateProjection<Inklio.Api.Domain.DeliveryImage, Inklio.Api.Application.Commands.DeliveryImage>();
        cfg.CreateProjection<Inklio.Api.Domain.Image, Inklio.Api.Application.Commands.Image>();
        cfg.CreateProjection<Inklio.Api.Domain.Tag, Inklio.Api.Application.Commands.Tag>();
    }).CreateMapper();

    private readonly IMapper mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Inklio.Api.Domain.Ask, Inklio.Api.Application.Commands.Ask>();
        cfg.CreateMap<Inklio.Api.Domain.AskComment, Inklio.Api.Application.Commands.AskComment>();
        cfg.CreateMap<Inklio.Api.Application.Commands.DeliveryCreateForm, Inklio.Api.Application.Commands.DeliveryCreateCommand>()
            .ForMember(x => x.Images, x => x.MapFrom(x => x.Images == null? null : new IFormFile[]{x.Images}));
        cfg.CreateMap<Inklio.Api.Domain.AskImage, Inklio.Api.Application.Commands.AskImage>();
        cfg.CreateMap<Inklio.Api.Domain.Comment, Inklio.Api.Application.Commands.Comment>();
        cfg.CreateMap<Inklio.Api.Domain.Delivery, Inklio.Api.Application.Commands.Delivery>();
        cfg.CreateMap<Inklio.Api.Domain.DeliveryComment, Inklio.Api.Application.Commands.DeliveryComment>();
        cfg.CreateMap<Inklio.Api.Domain.DeliveryImage, Inklio.Api.Application.Commands.DeliveryImage>();
        cfg.CreateMap<Inklio.Api.Domain.Image, Inklio.Api.Application.Commands.Image>();
        cfg.CreateMap<Inklio.Api.Application.Commands.AskCreateForm, Inklio.Api.Application.Commands.AskCreateCommand>()
            .ForMember(x => x.Images, x => x.MapFrom(x => x.Images == null? null : new IFormFile[]{x.Images}));
        cfg.CreateMap<Inklio.Api.Domain.Tag, Inklio.Api.Application.Commands.Tag>();
    }).CreateMapper();

    public AsksController(
        ILogger<AsksController> logger,
        IAskRepository askRepository,
        IMediator mediator,
        InklioContext context,
        IWebHostEnvironment hostEnvironment)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    [EnableQuery()]
    [HttpGet("v1/asks")]
    public IQueryable<Inklio.Api.Application.Commands.Ask> GetAsks()
    {
        var asks = projector.ProjectTo<Inklio.Api.Application.Commands.Ask>(this.askRepository.GetAsks());
        return asks;
    }

    [EnableQuery()]
    [HttpGet("v1/asks/{askId}")]
    public async Task<Inklio.Api.Application.Commands.Ask> GetAskById(
        int askId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var askDto = mapper.Map<Inklio.Api.Application.Commands.Ask>(ask);
        return askDto ?? throw new InvalidOperationException("Could not map Ask DTO");
    }

    [EnableQuery()]
    [HttpGet("v1/asks/{askId}/comments")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.AskComment>> GetComments(
        int askId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var comments = this.projector.ProjectTo<Inklio.Api.Application.Commands.AskComment>(ask.Comments.AsQueryable());
        return comments;
    }

    [EnableQuery()]
    [HttpGet("v1/asks/{askId}/deliveries")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.Delivery>> GetDeliveries(
        int askId,
        CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetAskByIdAsync(askId, cancellationToken);
        var deliveries = this.projector.ProjectTo<Inklio.Api.Application.Commands.Delivery>(ask.Deliveries.AsQueryable());
        return deliveries;
    }

    [EnableQuery()]
    [HttpGet("v1/asks/{askId}/deliveries/{deliveryId}")]
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

        var deliveryDto = mapper.Map<Inklio.Api.Application.Commands.Delivery>(delivery);
        return deliveryDto ?? throw new InvalidOperationException("Could not map Delivery DTO");
    }

    [EnableQuery()]
    [HttpGet("v1/asks/{askId}/deliveries/{deliveryId}/comments")]
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

        var comments = this.projector.ProjectTo<Inklio.Api.Application.Commands.DeliveryComment>(delivery.Comments.AsQueryable());

        return comments;
    }

    [HttpPost("v1/asks")]
    public async Task<IActionResult> AddAsk(
        [FromForm] AskCreateForm askCreateForm,
        CancellationToken cancellationToken)
    {
        var askCreateCommand = this.mapper.Map<AskCreateCommand>(askCreateForm);
        askCreateCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", askCreateCommand.GetGenericTypeName());
        await this.mediator.Send(askCreateCommand, cancellationToken);

        return this.Accepted();
    }

    [HttpPost("v1/asks/{askId}/comments")]
    public async Task AddAskComment(
        int askId,
        AskCommentCreateCommand commentCreateCommand,
        CancellationToken cancellationToken)
    {
        commentCreateCommand.AskId = askId;
        commentCreateCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", commentCreateCommand.GetGenericTypeName());
        await this.mediator.Send(commentCreateCommand, cancellationToken);
    }

    [HttpPost("v1/asks/{askId}/tags")]
    public async Task AddAskTag(
        int askId,
        AskTagAddCommand tagCommand,
        CancellationToken cancellationToken)
    {
        tagCommand.AskId = askId;
        tagCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", tagCommand.GetGenericTypeName());
        await this.mediator.Send(tagCommand, cancellationToken);
    }

    [HttpPost("v1/asks/{askId}/deliveries")]
    public async Task<IActionResult> AddDelivery(
        int askId,
        [FromForm] DeliveryCreateForm deliveryCreateForm,
        CancellationToken cancellationToken)
    {
        var deliveryCreateCommand = this.mapper.Map<DeliveryCreateCommand>(deliveryCreateForm);
        deliveryCreateCommand.AskId = askId;
        deliveryCreateCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        if (deliveryCreateForm.Images == null)
        {
            return this.BadRequest("Deliveries must contain a delivery image");
        }

        this.logger.LogInformation("----- Sending command: {CommandName}", deliveryCreateCommand.GetGenericTypeName());
        await this.mediator.Send(deliveryCreateCommand, cancellationToken);
        return this.Accepted();
    }

    [HttpPost("v1/asks/{askId}/deliveries/{deliveryId}/comments")]
    public async Task AddDeliveryComment(
        int askId,
        int deliveryId,
        DeliveryCommentCreateCommand commentCreateCommand,
        CancellationToken cancellationToken)
    {
        commentCreateCommand.AskId = askId;
        commentCreateCommand.DeliveryId = deliveryId;
        commentCreateCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", commentCreateCommand.GetGenericTypeName());
        await this.mediator.Send(commentCreateCommand, cancellationToken);
    }

    [HttpPost("v1/asks/{askId}/deliveries/{deliveryId}/tags")]
    public async Task AddDeliveryTag(
        int askId,
        int deliveryId,
        DeliveryTagAddCommand tagCommand,
        CancellationToken cancellationToken)
    {
        tagCommand.AskId = askId;
        tagCommand.DeliveryId = deliveryId;
        tagCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", tagCommand.GetGenericTypeName());
        await this.mediator.Send(tagCommand, cancellationToken);
    }
}
