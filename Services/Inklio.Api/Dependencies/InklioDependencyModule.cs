using Azure.Storage.Blobs;
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

        string sqlConnectionString = this.configuration.GetConnectionString("InklioSqlConnectionString");
        if (this.hostEnvironment.IsDevelopment() && string.IsNullOrWhiteSpace(sqlConnectionString))
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
            builder.Register<DbContextOptions<InklioContext>>((context) =>
            {
                var optionBuilder = new DbContextOptionsBuilder<InklioContext>()
                    .UseSnakeCaseNamingConvention()
                    .UseSqlServer(sqlConnectionString, sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });

                if (this.hostEnvironment.IsDevelopment())
                {
                    optionBuilder.LogTo(Console.WriteLine);
                }

                return optionBuilder.Options;
            }).SingleInstance();
        }

        builder.Register<BlobServiceClient>(ctx => new BlobServiceClient(this.configuration.GetConnectionString("InklioStorageConnectionString"))).SingleInstance();
        builder.RegisterType<InklioContext>().InstancePerLifetimeScope();
        builder.RegisterType<AskRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<BlobRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<TagRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<UserRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterMediatR(typeof(Program).Assembly);
        builder.RegisterModule(new MediatorDependencyModule());
    }
}