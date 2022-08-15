using Inklio.Api.Infrastructure;
using Autofac;
using Microsoft.EntityFrameworkCore;

public class InklioDependencyModule : Module
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
        });
        builder.RegisterType<InklioContext>();
    }
}