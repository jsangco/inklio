using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Infrastructure.Repositories;

public class AskRepository : IAskRepository
{
    private readonly InklioContext context;
    public IUnitOfWork UnitOfWork => this.context;

    /// <summary>
    /// Initialize of a new instance of a <see cref="AskRepository"/> object
    /// </summary>
    /// <param name="context">The db context.</param>
    public AskRepository(InklioContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<Ask> AddAskAsync(Ask ask, CancellationToken cancellationToken)
    {
        var addedAsk = await this.context.Asks.AddAsync(ask, cancellationToken);
        return addedAsk.Entity;
    }

    /// <inheritdoc/>
    public void Update(Ask ask)
    {
        this.context.Entry(ask).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    }

    /// <inheritdoc/>
    public IQueryable<Ask> GetAsks()
    {
        return this.context.Asks
            .Where(a => a.IsDeleted == false)
            .Include(e => e.Tags)
            .Include(e => e.Comments)
            .Include(e => e.Deliveries)
            .Include(e => e.Deliveries).ThenInclude(e => e.Images)
            .Include(e => e.Deliveries).ThenInclude(e => e.Tags);
    }

    /// <inheritdoc/>
    public async Task<Ask> GetAskByIdAsync(int askId, CancellationToken cancellationToken)
    {
        Ask? ask = await this.context.Asks
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .Include(a => a.Deliveries)
            .Include(a => a.Deliveries).ThenInclude(d => d.Images)
            .Include(e => e.Deliveries).ThenInclude(e => e.Tags)
            .FirstOrDefaultAsync(a => a.Id == askId, cancellationToken);

        if (ask is not null)
        {
            return ask;
        }

        throw new InklioDomainException(404, $"The specified Ask {askId} was not found");
    }

    /// <inheritdoc/>
    public bool TryGetTagByName(string type, string value, out Tag? tag)
    {
        tag = this.context.Tags.FirstOrDefault(t => t.Type == type && t.Value == value);
        return tag is not null;
    }
}
