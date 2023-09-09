using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InklioContext context;

    public IUnitOfWork UnitOfWork => this.context;

    /// <summary>
    /// Initialize of a new instance of a <see cref="UserRepository"/> object
    /// </summary>
    /// <param name="context">The db context.</param>
    public UserRepository(InklioContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<User> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        User? user = await this.context.Users.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

        if (user is not null)
        {
            return user;
        }

        throw new InklioDomainException(404, $"The specified User {userId} was not found");
    }

    /// <inheritdoc/>
    public async Task AddUserAsync(UserId userId, string username, CancellationToken cancellationToken)
    {
        User? userById = await this.context.Users.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);
        if (userById is not null)
        {
            throw new InvalidOperationException($"Cannot add user. User {userId} already exists.");
        }

        User? userByName = await this.context.Users.FirstOrDefaultAsync(a => a.Username == username, cancellationToken);
        if (userByName is not null)
        {
            throw new InvalidOperationException($"Cannot add user. User {username} already exists.");
        }

        var userToAdd = new User(userId, username);
        await this.context.Users.AddAsync(userToAdd, cancellationToken);
    }
}
