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
    public IQueryable<AskQueryObject> GetAsks(UserId? userId)
    {
        if (userId is null)
        {
            return this.context.Asks
                .Where(a => a.IsDeleted == false)
                .Select(a => new AskQueryObject() { Ask = a, IsUpvoted = false });
        }
        else
        {
            var upvoteUserId = this.context.Users.First(u => u.UserId == userId).Id;
            return this.context.Asks
                .Where(a => a.IsDeleted == false)
                .Include(a => a.Upvotes)
                .Select(a => new AskQueryObject() { Ask = a, IsUpvoted = a.Upvotes.Any(u => u.CreatedById == upvoteUserId) });
        }
    }

    /// <inheritdoc/>
    public async Task<Ask> GetAskByIdAsync(int askId, CancellationToken cancellationToken)
    {
        Ask? ask = await this.context.Asks
            .Where(a => a.IsDeleted == false)
            .Include(a => a.Comments)
            .Include(a => a.Comments).ThenInclude(e => e.Upvotes)
            .Include(a => a.Images)
            .Include(a => a.Tags)
            .Include(a => a.Upvotes)
            .Include(a => a.Deliveries)
            .Include(a => a.Deliveries).ThenInclude(d => d.Comments)
            .Include(a => a.Deliveries).ThenInclude(d => d.Comments).ThenInclude(c => c.Upvotes)
            .Include(a => a.Deliveries).ThenInclude(d => d.Images)
            .Include(a => a.Deliveries).ThenInclude(e => e.Tags)
            .Include(a => a.Deliveries).ThenInclude(e => e.Upvotes)
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
