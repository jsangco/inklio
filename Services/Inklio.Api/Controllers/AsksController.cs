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

namespace Inklio.Api.Controllers;

[ApiController]
[Route("asks")]
public class AsksController : ControllerBase
{
    private readonly ILogger<AsksController> logger;
    private readonly IAskRepository askRepository;
    private readonly IMediator mediator;
    private readonly InklioContext context;
    private readonly IMapper projector = new MapperConfiguration(cfg =>
    {
        cfg.CreateProjection<Inklio.Api.Domain.Ask, Inklio.Api.Application.Commands.Ask>()
            .ForMember(x => x.Comments, x => x.ExplicitExpansion())
            .ForMember(x => x.Deliveries, x => x.ExplicitExpansion());
        cfg.CreateProjection<Inklio.Api.Domain.AskComment, Inklio.Api.Application.Commands.AskComment>();
        cfg.CreateProjection<Inklio.Api.Domain.Comment, Inklio.Api.Application.Commands.Comment>();
        cfg.CreateProjection<Inklio.Api.Domain.DeliveryComment, Inklio.Api.Application.Commands.DeliveryComment>();
        cfg.CreateProjection<Inklio.Api.Domain.Delivery, Inklio.Api.Application.Commands.Delivery>();
        cfg.CreateProjection<Inklio.Api.Domain.Tag, Inklio.Api.Application.Commands.Tag>();
    }).CreateMapper();

    private readonly IMapper mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Inklio.Api.Domain.Ask, Inklio.Api.Application.Commands.Ask>();
        cfg.CreateMap<Inklio.Api.Domain.AskComment, Inklio.Api.Application.Commands.AskComment>();
        cfg.CreateMap<Inklio.Api.Domain.DeliveryComment, Inklio.Api.Application.Commands.DeliveryComment>();
        cfg.CreateMap<Inklio.Api.Domain.Delivery, Inklio.Api.Application.Commands.Delivery>();
        cfg.CreateMap<Inklio.Api.Domain.Tag, Inklio.Api.Application.Commands.Tag>();
    }).CreateMapper();

    public AsksController(ILogger<AsksController> logger, IAskRepository askRepository, IMediator mediator, InklioContext context)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.context = context;
    }

    [HttpGet]
    [EnableQueryWithCount()]
    public IQueryable<Inklio.Api.Application.Commands.Ask> GetAsk(ODataQueryOptions options)
    {
        var expandStr = options.SelectExpand?.RawExpand?.ToLowerInvariant() ?? string.Empty;
        var expansions = new List<Expression<Func<Application.Commands.Ask, object>>>();
        if (expandStr.Contains("comments"))
        {
            expansions.Add(x => x.Comments);
        }
        if (expandStr.Contains("deliver"))
        {
            expansions.Add(x => x.Deliveries);
        }

        var asks = projector.ProjectTo<Inklio.Api.Application.Commands.Ask>(this.askRepository.Get(),
            null,
            expansions.ToArray());
        return asks;
    }

    [HttpGet]
    [Route("{askId}")]
    [EnableQueryWithCount()]
    public async Task<Inklio.Api.Application.Commands.Ask> GetAskById(int askId, CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetByIdAsync(askId, cancellationToken);
        var askDto = mapper.Map<Inklio.Api.Application.Commands.Ask>(ask);
        return askDto ?? throw new InvalidOperationException("Could not map Ask DTO");
    }

    [HttpGet]
    [Route("{askId}/comments")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.AskComment>> GetComments(int askId, CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetByIdAsync(askId, cancellationToken);
        var comments = this.projector.ProjectTo<Inklio.Api.Application.Commands.AskComment>(ask.Comments.AsQueryable());
        return comments;
    }

    [HttpGet]
    [Route("{askId}/deliveries")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.Delivery>> GetDeliveries(int askId, CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetByIdAsync(askId, cancellationToken);
        var deliveries = this.projector.ProjectTo<Inklio.Api.Application.Commands.Delivery>(ask.Deliveries.AsQueryable());
        return deliveries;
    }

    [HttpGet]
    [Route("{askId}/deliveries/{deliveryId}")]
    [EnableQueryWithCount()]
    public async Task<Inklio.Api.Application.Commands.Delivery> GetDeliveryById(int askId, int deliveryId, CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetByIdAsync(askId, cancellationToken);
        var delivery = ask.Deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(404, $"Delivery {deliveryId} could not be found");
        }

        var deliveryDto = mapper.Map<Inklio.Api.Application.Commands.Delivery>(delivery);
        return deliveryDto ?? throw new InvalidOperationException("Could not map Delivery DTO");
    }

    [HttpGet]
    [Route("{askId}/deliveries/{deliveryId}/comments")]
    public async Task<IQueryable<Inklio.Api.Application.Commands.DeliveryComment>> GetDeliveryComments(int askId, int deliveryId, CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetByIdAsync(askId, cancellationToken);
        var delivery = ask.Deliveries.FirstOrDefault(d => d.Id == deliveryId);
        if (delivery is null)
        {
            throw new InklioDomainException(404, $"Delivery {deliveryId} could not be found");
        }

        var comments = this.projector.ProjectTo<Inklio.Api.Application.Commands.DeliveryComment>(delivery.Comments.AsQueryable());

        return comments;
    }

    [HttpPost]
    [Route("")]
    public async Task AddAsk(AskCreateCommand askCreateCommand, CancellationToken cancellationToken)
    {
        askCreateCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", askCreateCommand.GetGenericTypeName());
        await this.mediator.Send(askCreateCommand, cancellationToken);
    }

    [HttpPost]
    [Route("{askId}/comments")]
    public async Task AddAskComment(int askId, AskCommentCreateCommand commentCreateCommand, CancellationToken cancellationToken)
    {
        commentCreateCommand.AskId = askId;
        commentCreateCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", commentCreateCommand.GetGenericTypeName());
        await this.mediator.Send(commentCreateCommand, cancellationToken);
    }

    [HttpPost]
    [Route("{askId}/tags")]
    public async Task AddAskTag(int askId, AskTagAddCommand tagCommand, CancellationToken cancellationToken)
    {
        tagCommand.AskId = askId;
        tagCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", tagCommand.GetGenericTypeName());
        await this.mediator.Send(tagCommand, cancellationToken);
    }

    [HttpPost]
    [Route("{askId}/deliveries")]
    public async Task AddDelivery(int askId, DeliveryCreateCommand deliveryCreateCommand, CancellationToken cancellationToken)
    {
        deliveryCreateCommand.AskId = askId;
        deliveryCreateCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", deliveryCreateCommand.GetGenericTypeName());
        await this.mediator.Send(deliveryCreateCommand, cancellationToken);
    }

    [HttpPost]
    [Route("{askId}/deliveries/{deliveryId}/comments")]
    public async Task AddDeliveryComment(int askId, int deliveryId, DeliveryCommentCreateCommand commentCreateCommand, CancellationToken cancellationToken)
    {
        commentCreateCommand.AskId = askId;
        commentCreateCommand.DeliveryId = deliveryId;
        commentCreateCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", commentCreateCommand.GetGenericTypeName());
        await this.mediator.Send(commentCreateCommand, cancellationToken);
    }

    [HttpPost]
    [Route("{askId}/deliveries/{deliveryId}")]
    public async Task AddDeliveryTag(int askId, int deliveryId, DeliveryTagAddCommand tagCommand, CancellationToken cancellationToken)
    {
        tagCommand.AskId = askId;
        tagCommand.DeliveryId = deliveryId;
        tagCommand.UserId = Inklio.Api.Domain.User.TemporaryGlobalUserId;

        this.logger.LogInformation("----- Sending command: {CommandName}", tagCommand.GetGenericTypeName());
        await this.mediator.Send(tagCommand, cancellationToken);
    }
}
