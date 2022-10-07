using Inklio.Api.Domain;
using Inklio.Api.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inklio.Api.Infrastructure.EFCore;

public sealed class InklioContext : DbContext, IUnitOfWork
{
    public static string DbSchema { get; private set; } = "inklio";
    
    private readonly IMediator mediator;

    public DbSet<Ask> Asks  => Set<Ask>();
    public DbSet<AskComment> AskComments  => Set<AskComment>();
    public DbSet<Comment> Comments  => Set<Comment>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<DeliveryComment> DeliveryComments  => Set<DeliveryComment>();
    public DbSet<User> Users  => Set<User>();
    public DbSet<Tag> Tags  => Set<Tag>();

    public InklioContext(DbContextOptions<InklioContext> options, IMediator mediator) : base(options)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Set the global db schema. The default is "inklio" and targets the production environment. This value
    /// can be changed to target a staging environment schema.
    /// </summary>
    /// <param name="dbSchema">The schema to change to</param>
    public static void SetDbSchema(string dbSchema)
    {
        DbSchema = dbSchema;
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await this.mediator.DispatchDomainEventsAsync(this);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        var result = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AskEntityTypeConfiguration());
        builder.ApplyConfiguration(new AskCommentEntityTypeConfiguration());
        builder.ApplyConfiguration(new AskImageEntityTypeConfiguration());
        builder.ApplyConfiguration(new CommentEntityTypeConfiguration());
        builder.ApplyConfiguration(new DeliveryEntityTypeConfiguration());
        builder.ApplyConfiguration(new DeliveryCommentEntityTypeConfiguration());
        builder.ApplyConfiguration(new DeliveryImageEntityTypeConfiguration());
        builder.ApplyConfiguration(new ImageEntityTypeConfiguration());
        builder.ApplyConfiguration(new TagEntityTypeConfiguration());
        builder.ApplyConfiguration(new UpvoteEntityTypeConfiguration());
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyFlagEntityConfiguration();


    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder
            .Properties<DateTime>()
            .HaveConversion(typeof(UtcValueConverter));
    }
}
