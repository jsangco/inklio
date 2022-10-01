using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An interface for an <see cref="User"/> repository
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Adds an <see cref="User"/> to the repository.
    /// </summary>
    /// <param name="user">The user to add.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The added user</returns>
    Task<User> AddAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an <see cref="User"/> in the repository.
    /// </summary>
    /// <param name="user">The user to update.</param>
    void Update(User user);

    /// <summary>
    /// Gets all <see cref="User"/> objects from the repository.
    /// </summary>
    /// <returns>All user obojects</returns>
    IQueryable<User> Get();

    /// <summary>
    /// Gets an <see cref="User"/> from the repository.
    /// </summary>
    /// <param name="userId">The id of the user to get.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The user.</returns>
    Task<User> GetByIdAsync(int userId, CancellationToken cancellationToken);
}