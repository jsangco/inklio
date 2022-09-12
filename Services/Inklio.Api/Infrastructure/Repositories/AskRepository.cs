using Inklio.Api.Domain;
using Inklio.Api.SeedWork;

namespace Inklio.Api.Infrastructure.Repositories;

public class AskRepository : IAskRepository
{
    private readonly InklioContext context;

    /// <inheritdoc/>
    public IUnitOfWork MyProperty => context;

    public IUnitOfWork UnitOfWork => throw new NotImplementedException();

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
        return this.context.Asks;
    }

    /// <inheritdoc/>
    public async Task<Ask?> GetByIdAsync(int askId, CancellationToken cancellationToken)
    {
        var ask = await this.context.Asks.FindAsync(askId, cancellationToken);
        return ask;
    }
}