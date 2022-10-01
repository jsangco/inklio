using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// An interface for an <see cref="Tag"/> repository
/// </summary>
public interface ITagRepository : IRepository<Tag>
{
    /// <summary>
    /// Gets a <see cref="Tag"/> objects from the repository based on the type and vaule. If no tag exsists, a new tag is created.
    /// </summary>
    /// <returns>All tag obojects</returns>
    Tag GetOrCreate(string type, string value);
}