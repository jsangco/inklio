using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inklio.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AsksController : ControllerBase
{
    private readonly ILogger<AsksController> logger;
    private readonly IAskRepository askRepository;
    private readonly MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => {
        cfg.CreateProjection<Inklio.Api.Domain.Ask, Inklio.Api.Models.Ask>();
    });

    public AsksController(ILogger<AsksController> logger, IAskRepository askRepository)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.askRepository = askRepository ?? throw new ArgumentNullException(nameof(askRepository));
    }

    [HttpGet]
    [EnableQuery(PageSize=2)]
    public IQueryable<Inklio.Api.Models.Ask>? Get()
    {
        return this.askRepository.Get().ProjectTo<Inklio.Api.Models.Ask>(this.mapperConfiguration);
    }

    public void Post(AskCreate askCreate)
}
