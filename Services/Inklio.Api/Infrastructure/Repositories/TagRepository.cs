using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly InklioContext context;
    public IUnitOfWork UnitOfWork => this.context;

    /// <summary>
    /// Initialize of a new instance of a <see cref="TagRepository"/> object
    /// </summary>
    /// <param name="context">The db context.</param>
    public TagRepository(InklioContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task AddAsync(Tag ask, CancellationToken cancellationToken)
    {
        await this.context.Tags.AddAsync(ask, cancellationToken);
    }

    /// <inheritdoc/>
    public void Delete(string type, string value)
    {
        var tag = this.context.Tags.FirstOrDefault(t => t.Type == type && t.Value == value);
        if (tag is null)
        {
            throw new InklioDomainException(404, $"Could not find tag {type}:{value}");
        }

        this.context.Tags.Remove(tag);
    }

    /// <inheritdoc/>
    public bool TryGetByName(string type, string value, out Tag? tag)
    {
        tag = this.context.Tags.FirstOrDefault(t => t.Type == type && t.Value == value);
        return tag is not null;
    }
}
