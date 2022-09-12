using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inklio.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveriesController : ControllerBase
{
    private readonly ILogger<DeliveriesController> logger;
    private readonly IMediator mediator;

    public DeliveriesController(ILogger<DeliveriesController> logger, IMediator mediator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public int Get()
    {
        return 0;
    }
}
