using Art.Api.HealthCheck;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var appBuilder = WebApplication.CreateBuilder(args);
appBuilder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer((ContainerBuilder builder) =>
    {
        builder.RegisterModule<ArtDependencyModule>();
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
    AllowCachingResponses = false,
    Predicate = healthCheck => healthCheck.Name == "Database"
});

app.Run();
