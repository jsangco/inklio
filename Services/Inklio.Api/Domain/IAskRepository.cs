using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An interface for an <see cref="Ask"/> repository
/// </summary>
public interface IAskRepository : IRepository<Ask>
{
    /// <summary>
    /// Adds an <see cref="Ask"/> to the repository.
    /// </summary>
    /// <param name="ask">The ask to add.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The added ask</returns>
    Task<Ask> AddAskAsync(Ask ask, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all <see cref="Ask"/> objects from the repository.
    /// </summary>
    /// <param name="userId">An optional user Id associated with the query.</param>
    /// <returns>All ask obojects</returns>
    IQueryable<AskQueryObject> GetAsks(UserId? userId);

    /// <summary>
    /// Gets all <see cref="Ask"/> objects from the repository and includes deleted
    /// enteries.
    /// </summary>
    /// <param name="userId">An optional user Id associated with the query.</param>
    /// <returns>All ask obojects</returns>
    IQueryable<AskQueryObject> GetAllAsks(UserId? userId);

    /// <summary>
    /// Gets an <see cref="Ask"/> from the repository.
    /// </summary>
    /// <param name="askId">The id of the ask to get.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The ask.</returns>
    Task<Ask> GetAskByIdAsync(int askId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an <see cref="Ask"/> from the repository and may include deleted
    /// enteries.
    /// </summary>
    /// <param name="askId">The id of the ask to get.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The ask.</returns>
    Task<Ask> GetAnyAskByIdAsync(int askId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an <see cref="Ask"/> from the repository. And includes information
    /// specific to the user making the query such as whether it has been
    /// upvoted by the user.
    /// </summary>
    /// <param name="askId">The id of the ask to get.</param>
    /// <param name="userId">The userId that is fetching the ask.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The ask.</returns>
    Task<Ask> GetAskByIdAsync(int askId, UserId? userId, CancellationToken cancellationToken);

    /// <summary>
    /// Attempts to fetcha a <see cref="Tag"/> from the tag repository.
    /// </summary>
    /// <param name="type">The type of the tag</param>
    /// <param name="value">The value of the tag</param>
    /// <param name="tag">An out parameter that contains the tag if it exists</param>
    /// <returns>A flag indicating if the Tag exists in the repository.</returns>
    bool TryGetTagByName(string type, string value, out Tag? tag);
}