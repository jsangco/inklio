using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AsksController : ControllerBase
{
    private readonly ILogger<AsksController> logger;
    private readonly IMediator mediator;

    private readonly MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => {
        cfg.CreateProjection<Inklio.Api.Domain.Ask, Inklio.Api.Models.Ask>();
    });

    // public IEnumerable<Inklio.Api.Models.Ask> Asks { get; set; }
    public IEnumerable<Inklio.Api.Domain.Ask> DomainAsks { get; set; }

    public AsksController(ILogger<AsksController> logger, IMediator mediator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.DomainAsks = new Inklio.Api.Domain.Ask[]
        {
            new Inklio.Api.Domain.Ask(){Id = 1,},
            new Inklio.Api.Domain.Ask(){Id = 2,},
            new Inklio.Api.Domain.Ask(){Id = 3,},
            new Inklio.Api.Domain.Ask(){Id = 4,},
            new Inklio.Api.Domain.Ask(){Id = 5,},
            new Inklio.Api.Domain.Ask(){Id = 6,},
        };
    }

    [HttpGet]
    [EnableQuery(PageSize=2)]
    public IQueryable<Inklio.Api.Models.Ask>? Get(ODataQueryOptions options)
    {
        var settings =  new ODataQuerySettings();
        settings.PageSize = 2;
        var result = options.ApplyTo(DomainAsks.AsQueryable(), settings);
        return (result as IQueryable<Inklio.Api.Models.Ask>).ProjectTo<Inklio.Api.Models.Ask>(this.mapperConfiguration);
    }
}
