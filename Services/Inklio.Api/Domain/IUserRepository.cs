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
    Task<User> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an <see cref="User"/> from the repository.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The user.</returns>
    Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the <see cref="User"/> objects from the repository
    /// </summary>
    /// <returns>A collection of users.</returns>
    IQueryable<User> GetUsers();

    /// <summary>
    /// Gets or adds a user to the repository.
    /// </summary>
    /// <param name="user">The user to get or add.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A user.</returns>
    Task<User> GetOrAddUserAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a user to the repository.
    /// </summary>
    /// <param name="userId">The <see cref="UserId"/> of the new user.</param>
    /// <param name="username">Th unique name of the user.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A Task.</returns>
    Task<User> AddUserAsync(UserId userId, string username, CancellationToken cancellationToken);
}