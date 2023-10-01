using AutoMapper;
using Azure.Storage.Blobs;
using Inklio.Api.Infrastructure.EFCore;
using Inklio.Api.Infrastructure.Repositories;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

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
        WebConfiguration webConfiguration = this.configuration.GetSection("Web").Get<WebConfiguration>() ?? throw new ArgumentNullException("Cannot load WebConfiguration");
        builder.Register<WebConfiguration>(ctx => webConfiguration);

        string? sqlConnectionString = this.configuration.GetConnectionString("InklioSqlConnectionString");
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

        string? storageConnectionString = this.configuration.GetConnectionString("InklioStorageConnectionString");
        builder.Register<BlobServiceClient>(ctx => new BlobServiceClient(storageConnectionString)).SingleInstance();
        builder.RegisterType<InklioContext>().InstancePerLifetimeScope();
        builder.RegisterType<AskRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<BlobRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<TagRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterType<UserRepository>().AsImplementedInterfaces().InstancePerLifetimeScope();
        builder.RegisterMediatR(
            MediatRConfigurationBuilder
                .Create(typeof(Program).Assembly)
                .WithAllOpenGenericHandlerTypesRegistered()
                .Build());
        builder.RegisterModule(new MediatorDependencyModule());
        builder.Register<IMapper>(ctx =>
        {
            var webConfig = ctx.Resolve<WebConfiguration>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new InklioAutoMapperProfile(new Uri(webConfig.ImagesUrl)));
            });
            return mapperConfig.CreateMapper();
        }).SingleInstance();
    }
}