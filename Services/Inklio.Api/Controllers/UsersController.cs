using AutoMapper;
using Inklio.Api.Domain;
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
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public UsersController(
        IUserRepository userRepository,
        IMapper mapper,
        IMediator mediator)
    {
        this.userRepository = userRepository;
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
}
