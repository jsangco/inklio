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
    Task<Ask> AddAsync(Ask ask, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all <see cref="Ask"/> objects from the repository.
    /// </summary>
    /// <returns>All ask obojects</returns>
    IQueryable<Ask> Get();

    /// <summary>
    /// Gets an <see cref="Ask"/> from the repository.
    /// </summary>
    /// <param name="askId">The id of the ask to get.</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The ask.</returns>
    Task<Ask> GetByIdAsync(int askId, CancellationToken cancellationToken);
}