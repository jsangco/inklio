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

    private readonly IMapper mapper = new MapperConfiguration(cfg => {
        cfg.CreateProjection<Inklio.Api.Domain.Ask, Inklio.Api.Application.Commands.Ask>();
    }).CreateMapper();

    public AsksController(ILogger<AsksController> logger, IAskRepository askRepository, IMediator mediator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [EnableQuery(PageSize=2)]
    public IQueryable<Inklio.Api.Application.Commands.Ask> Get()
    {
        return mapper.ProjectTo<Inklio.Api.Application.Commands.Ask>(this.askRepository.Get());
    }

    [HttpPost]
    public async Task<Inklio.Api.Application.Commands.Ask> Post(AskCreateCommand askCreateCommand, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("----- Sending command: {CommandName}", askCreateCommand.GetGenericTypeName());
        Inklio.Api.Application.Commands.Ask ask = await this.mediator.Send(askCreateCommand, cancellationToken);
        return ask;
    }
}
