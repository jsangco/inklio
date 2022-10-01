using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Infrastructure.Repositories;

public class AskRepository : IAskRepository
{
    private readonly InklioContext context;

    /// <inheritdoc/>
    public IUnitOfWork MyProperty => context;

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
    public async Task<Ask> AddAsync(Ask ask, CancellationToken cancellationToken)
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
    public IQueryable<Ask> Get()
    {
        return this.context.Asks
            .Include(e => e.Tags)
            .Include(e => e.Comments)
            .Include(e => e.Deliveries);
    }

    /// <inheritdoc/>
    public async Task<Ask> GetByIdAsync(int askId, CancellationToken cancellationToken)
    {
        Ask? ask = await this.context.Asks
            .Include(a => a.Tags)
            .Include(a => a.Comments)
            .Include(a => a.Deliveries)
            .FirstOrDefaultAsync(a => a.Id == askId, cancellationToken);

        if (ask is not null)
        {
            return ask;
        }

        throw new InklioDomainException(404, $"The specified Ask {askId} was not found");
    }
}
