using Inklio.Api.Infrastructure.EFCore;
using Inklio.Api.Infrastructure.Repositories;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace Inklio.Api.Dependencies;

public class InklioDependencyModule : Autofac.Module
{
    private readonly IConfiguration configuration;
    private readonly IHostEnvironment hostEnvironment;

    public InklioDependencyModule(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
    }

    /// <inheritdoc/>
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MyService>().AsSelf();

        if (this.hostEnvironment.IsDevelopment() && false)
        {
            builder.Register<DbContextOptions<InklioContext>>((context) =>
            {
                return new DbContextOptionsBuilder<InklioContext>()
                    .UseSnakeCaseNamingConvention()
                    .UseInMemoryDatabase("InklioTestDatabase").Options;
            }).SingleInstance();
        }
        else
        {
            // string connectionString = this.configuration.GetConnectionString("InklioSqlConnectionString");
            string connectionString = "Server=tcp:inklio.database.windows.net,1433;Initial Catalog=inklio;Persist Security Info=False;User ID=adminaoeu;Password=d4hQqkHbtue7fDbk54dGdhb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            builder.Register<DbContextOptions<InklioContext>>((context) =>
            {
                return new DbContextOptionsBuilder<InklioContext>()
                    .UseSnakeCaseNamingConvention()
                    .UseSqlServer(connectionString, sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }).Options;
            }).SingleInstance();
        }
        builder.RegisterType<InklioContext>().InstancePerLifetimeScope();
        builder.RegisterType<AskRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterMediatR(typeof(Program).Assembly);
        builder.RegisterModule(new MediatorDependencyModule());
    }
}