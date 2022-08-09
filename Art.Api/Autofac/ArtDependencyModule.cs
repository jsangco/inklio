using Art.Api.Infrastructure;
using Autofac;
using Microsoft.EntityFrameworkCore;

public class ArtDependencyModule : Module
{
    private readonly IConfiguration configuration;

    public ArtDependencyModule(IConfiguration configuration)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <inheritdoc/>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MyService>().AsSelf();

        string connectionString = this.configuration.GetConnectionString("ArtSqlConnectionString");
        builder.Register<DbContextOptions<ArtContext>>((context) =>
        {
            return new DbContextOptionsBuilder<ArtContext>()
                .UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }).Options;
        });
        builder.RegisterType<ArtContext>();
    }
}