using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An interface for an <see cref="User"/> repository
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Gets an <see cref="User"/> from the repository.
    /// </summary>
    /// <param name="userId">The id of the user to get.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The user.</returns>
    Task<User> GetByIdAsync(int userId, CancellationToken cancellationToken);
}