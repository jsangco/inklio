using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An interface for an <see cref="Tag"/> repository
/// </summary>
public interface ITagRepository : IRepository<Tag>
{
    /// <summary>
    /// Adds a tag to the repository.
    /// </summary>
    /// <param name="tag">The tag to add</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task AddAsync(Tag tag, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a tag.
    /// </summary>
    /// <param name="type">The type of the tag</param>
    /// <param name="value">The value of the tag</param>
    void Delete(string type, string value);

    /// <summary>
    /// Attempts to fetcha a <see cref="Tag"/> from the tag repository.
    /// </summary>
    /// <param name="type">The type of the tag</param>
    /// <param name="value">The value of the tag</param>
    /// <param name="tag">An out parameter that contains the tag if it exists</param>
    /// <returns>A flag indicating if the Tag exists in the repository.</returns>
    bool TryGetByName(string type, string value, out Tag? tag);
}