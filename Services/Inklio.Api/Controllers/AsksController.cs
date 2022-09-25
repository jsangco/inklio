using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inklio.Api.Application.Commands;
using Inklio.Api.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Inklio.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AsksController : ControllerBase
{
    private readonly ILogger<AsksController> logger;
    private readonly IAskRepository askRepository;
    private readonly IMediator mediator;

    private readonly IMapper projector = new MapperConfiguration(cfg =>
    {
        cfg.CreateProjection<Inklio.Api.Domain.Ask, Inklio.Api.Application.Commands.Ask>();
        cfg.CreateProjection<Inklio.Api.Domain.Comment, Inklio.Api.Application.Commands.Comment>();
        cfg.CreateProjection<Inklio.Api.Domain.Delivery, Inklio.Api.Application.Commands.Delivery>();
    }).CreateMapper();

    private readonly IMapper mapper = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Inklio.Api.Domain.Ask, Inklio.Api.Application.Commands.Ask>();
        cfg.CreateMap<Inklio.Api.Domain.Comment, Inklio.Api.Application.Commands.Comment>();
        cfg.CreateMap<Inklio.Api.Domain.Delivery, Inklio.Api.Application.Commands.Delivery>();
    }).CreateMapper();

    public AsksController(ILogger<AsksController> logger, IAskRepository askRepository, IMediator mediator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [EnableQuery()]
    public IQueryable<Inklio.Api.Application.Commands.Ask> Get()
    {
        var asks = projector.ProjectTo<Inklio.Api.Application.Commands.Ask>(this.askRepository.Get());
        return asks;
    }

    [HttpGet]
    [Route("{id}")]
    [EnableQuery()]
    public async Task<Inklio.Api.Application.Commands.Ask> GetById(int id, CancellationToken cancellationToken)
    {
        var ask = await this.askRepository.GetByIdAsync(id, cancellationToken);
        var askDto = mapper.Map<Inklio.Api.Application.Commands.Ask>(ask);
        return askDto ?? throw new InvalidOperationException("Could not map Ask DTO");
    }

    [HttpPost]
    public async Task<Inklio.Api.Application.Commands.Ask> Post(AskCreateCommand askCreateCommand, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("----- Sending command: {CommandName}", askCreateCommand.GetGenericTypeName());
        Inklio.Api.Application.Commands.Ask ask = await this.mediator.Send(askCreateCommand, cancellationToken);
        return ask;
    }

    [HttpPost]
    public async Task<Inklio.Api.Application.Commands.Ask> AddComment(AskCreateCommand askCreateCommand, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("----- Sending command: {CommandName}", askCreateCommand.GetGenericTypeName());
        Inklio.Api.Application.Commands.Ask ask = await this.mediator.Send(askCreateCommand, cancellationToken);
        return ask;
    }
}
