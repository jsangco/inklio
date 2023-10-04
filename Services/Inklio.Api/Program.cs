using Inklio.Api.HealthCheck;
using System.Text.Json.Serialization;
using Inklio.Api.Startup;
using Inklio.Api.Dependencies;
using Inklio.Api.Infrastructure.Filters;

Console.WriteLine("Starting Inklio Api");
var appBuilder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Environment: {appBuilder.Environment.EnvironmentName}");

appBuilder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration((HostBuilderContext hostContext, IConfigurationBuilder configBuilder) =>
    {
        configBuilder.AddEnvironmentVariables();
    })
    .ConfigureContainer((HostBuilderContext hostContext, ContainerBuilder builder) =>
    {
        builder.RegisterModule(new InklioDependencyModule(hostContext.Configuration, hostContext.HostingEnvironment));
    });

// Configure Services

appBuilder.Services.PostConfigure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory.Instance;
});

appBuilder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpGlobalExceptionFilter>();
})
.AddApiOData()
.AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    jsonOptions.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
});
appBuilder.Services.AddEndpointsApiExplorer();
appBuilder.Services.AddSwaggerGen( c =>
{
    c.ResolveConflictingActions(api => api.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomOperationIds(e => $"{e.RelativePath?.Replace("/","_").Replace("$","_")}_{e.HttpMethod}");
});
appBuilder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");

string identitySqlConnectionString = appBuilder.Configuration.GetConnectionString("InklioSqlConnectionString")
    ?? throw new ArgumentNullException("InklioSqlConnectionString");
appBuilder.Services.AddInklioIdentity(appBuilder.Environment, identitySqlConnectionString);

var app = appBuilder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment()) Leaving swagger on for now
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.UseODataExceptionHandler(appBuilder.Environment);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestBaseUrlRewriter(appBuilder.Configuration.GetSection("Web").Get<WebConfiguration>() ?? throw new ArgumentNullException("Cannot load WebConfiguration"));
app.MapControllers();
app.MapHealthChecks("/", new HealthCheckOptions
{
    ResponseWriter = HealthCheckWriter.WriteResponse,
    AllowCachingResponses = false,
    Predicate = healthCheck => healthCheck.Name == "Database",
});
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = HealthCheckWriter.WriteResponse,
    AllowCachingResponses = false,
    Predicate = healthCheck => healthCheck.Name == "Database",
});

Console.WriteLine($"Starting Inklio API. Version: {HealthCheckWriter.GetAppVersion()}, Environment: {app.Environment.EnvironmentName}");
app.Run();