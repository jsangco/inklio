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

        if (this.hostEnvironment.IsDevelopment() )
        {
            builder.Register<DbContextOptions<InklioContext>>((context) =>
            {
                return new DbContextOptionsBuilder<InklioContext>().UseInMemoryDatabase("InklioTestDatabase").Options;
            }).SingleInstance();
        }
        else
        {
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
        }
        builder.RegisterType<InklioContext>().InstancePerLifetimeScope();
        builder.RegisterType<AskRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterMediatR(typeof(Program).Assembly);
        builder.RegisterModule(new MediatorDependencyModule());
    }
}