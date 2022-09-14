using Inklio.Api.Domain;
using Inklio.Api.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inklio.Api.Infrastructure.EFCore;

public sealed class InklioContext : DbContext, IUnitOfWork
{
    public const string DefaultDbSchema = "inklio";
    
    private readonly IMediator mediator;

    public DbSet<Ask> Asks  => Set<Ask>();
    public DbSet<AskComment> AskComments  => Set<AskComment>();
    public DbSet<Comment> Comments  => Set<Comment>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<DeliveryComment> DeliveryComments  => Set<DeliveryComment>();

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AskEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AskCommentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryCommentEntityTypeConfiguration());
    }
}