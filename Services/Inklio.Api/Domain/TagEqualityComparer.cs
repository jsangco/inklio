using System.Diagnostics.CodeAnalysis;

namespace Inklio.Api.Domain;

/// <summary>
/// An equality comparison for tags
/// </summary>
public class TagEqualityComparer : IEqualityComparer<Tag>
{
    /// <inheritdoc/>
    public bool Equals(Tag? x, Tag? y)
    {
        return x?.ToString() == y?.ToString();
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] Tag obj)
    {
        return obj.ToString().GetHashCode();
    }
}