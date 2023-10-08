using AutoMapper;
using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Api.Controllers;

[ApiController]
[Route("v1/users")]
public class UsersController : ODataController
{
    private readonly IUserRepository userRepository;
    private readonly InklioContext inklioContext;
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public UsersController(
        IUserRepository userRepository,
        InklioContext inklioContext,
        IMapper mapper,
        IMediator mediator)
    {
        this.userRepository = userRepository;
        this.inklioContext = inklioContext;
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [EnableQuery]
    [HttpGet]
    public IQueryable<Inklio.Api.Application.Commands.User> GetUsers()
    {
        var users = this.mapper.ProjectTo<Inklio.Api.Application.Commands.User>(this.userRepository.GetUsers());
        return users;
    }

    [EnableQuery]
    [HttpGet("{username}")]
    public async Task<Inklio.Api.Application.Commands.User> GetUser(string username, CancellationToken cancellationToken)
    {
        var getUser = async () =>
        {
            if (Guid.TryParse(username, out UserId userId))
            {
                return await this.userRepository.GetByUserIdAsync(userId, cancellationToken);
            }
            return await this.userRepository.GetByUsernameAsync(username, cancellationToken);
        };
        var user = await getUser();
        var userDto = this.mapper.Map<Inklio.Api.Application.Commands.User>(user);
        return userDto ?? throw new InvalidOperationException("Could not map User DTO");
    }

    [EnableQuery]
    [HttpGet("{username}/asks")]
    public async Task<IQueryable<CommandAsk>> GetUserAsks(string username, CancellationToken cancellationToken)
    {
        var user = await this.inklioContext.Users
            .FirstOrDefaultAsync(a => a.Username == username, cancellationToken);
        if (user is null)
        {
            throw new InklioDomainException(404, "Cannot find user");
        }

        var asks = this.inklioContext.Asks.Where(a => a.CreatedById == user.Id);
        return this.mapper.ProjectTo<CommandAsk>(asks);
    }

    [EnableQuery]
    [HttpGet("{username}/deliveries")]
    public async Task<IQueryable<CommandDelivery>> GetUserDeliveries(string username, CancellationToken cancellationToken)
    {
        var user = await this.inklioContext.Users
            .FirstOrDefaultAsync(a => a.Username == username, cancellationToken);
        if (user is null)
        {
            throw new InklioDomainException(404, "Cannot find user");
        }

        var deliveries = this.inklioContext.Deliveries.Where(a => a.CreatedById == user.Id);
        return this.mapper.ProjectTo<CommandDelivery>(deliveries);
    }
}
