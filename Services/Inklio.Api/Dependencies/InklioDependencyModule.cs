using Inklio.Api.Infrastructure;
using Inklio.Api.Infrastructure.Repositories;

namespace Inklio.Api.Dependencies;

public class InklioDependencyModule : Autofac.Module
{
    private readonly IConfiguration configuration;

    public InklioDependencyModule(IConfiguration configuration)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <inheritdoc/>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MyService>().AsSelf();

        string connectionString = this.configuration.GetConnectionString("InklioSqlConnectionString");
        builder.Register<DbContextOptions<InklioContext>>((context) =>
        {
            return new DbContextOptionsBuilder<InklioContext>()
                .UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }).Options;
        }).SingleInstance();
        builder.RegisterType<InklioContext>().InstancePerLifetimeScope();
        builder.RegisterType<AskRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
    }
}