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
[Route("v1/challenges")]
public class ChallengesController : ODataController
{
    private readonly ILogger<ChallengesController> logger;
    private readonly IAskRepository askRepository;
    private readonly IMediator mediator;
    private readonly IWebHostEnvironment hostEnvironment;
    private readonly IMapper mapper;

    public ChallengesController(
        ILogger<ChallengesController> logger,
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
    public IQueryable<Inklio.Api.Application.Commands.Challenge> GetChallenges()
    {
        var userId = this.User.UserIdOrDefault();
        var challenges = this.mapper.ProjectTo<Inklio.Api.Application.Commands.Challenge>(this.askRepository.GetChallenges());
        return challenges;
    }
}