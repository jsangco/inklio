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
    public async Task<User> GetByIdAsync(int userId, CancellationToken cancellationToken)
    {
        User? user = await this.context.Users.FirstOrDefaultAsync(a => a.Id == userId, cancellationToken);

        if (user is not null)
        {
            return user;
        }

        throw new InklioDomainException(404, $"The specified User {userId} was not found");
    }
}
