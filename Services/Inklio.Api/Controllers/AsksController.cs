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

    public IEnumerable<Inklio.Api.Models.Ask> Asks { get; set; }

    public AsksController(ILogger<AsksController> logger, IMediator mediator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.Asks = new Inklio.Api.Models.Ask[]
        {
            new Inklio.Api.Models.Ask(){Id = 1, Asdf = -1},
            new Inklio.Api.Models.Ask(){Id = 2, Asdf = -1},
            new Inklio.Api.Models.Ask(){Id = 3, Asdf = -1},
            new Inklio.Api.Models.Ask(){Id = 4, Asdf = -2},
            new Inklio.Api.Models.Ask(){Id = 5, Asdf = -2},
            new Inklio.Api.Models.Ask(){Id = 6, Asdf = -2},
        };
    }

    [HttpGet]
    [EnableQuery(PageSize=2)]
    public IEnumerable<Inklio.Api.Models.Ask> Get()
    {
        return this.Asks;
    }
}
