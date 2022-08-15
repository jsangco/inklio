using Inklio.Api.Domain;
using Inklio.Api.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inklio.Api.Infrastructure;

public sealed class InklioContext : DbContext, IUnitOfWork
{
    private readonly IMediator mediator;

    public DbSet<Ask>? Asks {get; set;}

    public InklioContext(DbContextOptions<InklioContext> options, IMediator mediator) : base(options)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        // await this.mediator.DispatchDomainEventsAsync(this);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}