using Inklio.Api.HealthCheck;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Inklio.Api.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Inklio.Api.Dependencies;
using Inklio.Api.Infrastructure.Filters;

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
        builder.RegisterModule(new InklioDependencyModule(hostContext.Configuration));
        builder.RegisterModule(new MediatorDependencyModule());
    });

appBuilder.Services.AddControllers( options =>
    {
        options.Filters.Add<HttpGlobalExceptionFilter>();
        options.Filters.Add<ValidateModelStateFilter>();
    })
    .AddApiOData()
    .AddJsonOptions(jsonOptions =>
    {
	    jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
	    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
	    jsonOptions.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    });
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
