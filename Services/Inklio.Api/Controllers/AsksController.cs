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
        cfg.CreateProjection<Inklio.Api.Domain.Comment, Inklio.Api.Application.Commands.Comment>();
        cfg.CreateProjection<Inklio.Api.Domain.Delivery, Inklio.Api.Application.Commands.Delivery>();
        cfg.CreateProjection<Inklio.Api.Domain.Tag, Inklio.Api.Application.Commands.Tag>();
    }).CreateMapper();

    private readonly IMapper mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Inklio.Api.Domain.Ask, Inklio.Api.Application.Commands.Ask>();
        cfg.CreateMap<Inklio.Api.Domain.Comment, Inklio.Api.Application.Commands.Comment>();
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
    public IQueryable<Inklio.Api.Application.Commands.Ask> Get(ODataQueryOptions options)
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
    [Route("{id}")]
    [EnableQueryWithCount()]
    public async Task<Inklio.Api.Application.Commands.Ask> GetById(int id, CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetByIdAsync(id, cancellationToken);
        var askDto = mapper.Map<Inklio.Api.Application.Commands.Ask>(ask);
        return askDto ?? throw new InvalidOperationException("Could not map Ask DTO");
    }

    [HttpPost]
    public async Task Post(AskCreateCommand askCreateCommand, CancellationToken cancellationToken)
    {
        askCreateCommand.UserId = 1;

        this.logger.LogInformation("----- Sending command: {CommandName}", askCreateCommand.GetGenericTypeName());
        await this.mediator.Send(askCreateCommand, cancellationToken);
    }

    [HttpPost]
    [Route("{id}/commands")]
    public async Task AddComment(AskCreateCommand askCreateCommand, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("----- Sending command: {CommandName}", askCreateCommand.GetGenericTypeName());
        await this.mediator.Send(askCreateCommand, cancellationToken);
    }
}
