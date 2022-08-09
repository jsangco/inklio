using Art.Api.HealthCheck;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var appBuilder = WebApplication.CreateBuilder(args);
appBuilder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration((HostBuilderContext hostContext, IConfigurationBuilder configBuilder) =>
    {
        configBuilder.AddEnvironmentVariables();
    })
    .ConfigureContainer((HostBuilderContext hostContext, ContainerBuilder builder) =>
    {
        builder.RegisterMediatR(typeof(Program).Assembly);
        builder.RegisterModule(new ArtDependencyModule(hostContext.Configuration));
    });

appBuilder.Services.AddControllers();
appBuilder.Services.AddEndpointsApiExplorer();
appBuilder.Services.AddSwaggerGen();
appBuilder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");

var app = appBuilder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/", new HealthCheckOptions
{
    ResponseWriter = HealthCheckWriter.WriteResponse,
    AllowCachingResponses = false,
    Predicate = healthCheck => healthCheck.Name == "Database",
});

Console.WriteLine("Starting application. Version: " + HealthCheckWriter.GetAppVersion());
app.Run();
