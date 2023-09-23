using System.Runtime.CompilerServices;
using System.Text.Json;
using Inklio.Api.Application.Commands;
using Microsoft.AspNetCore.OData;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Inklio.Api.Startup;

public static class ODataExtensions
{
    /// <summary>
    /// A middleware that changes the base of the Request URL for every incoming request to to the ApiUrl in a <see cref="WebConfiguration"/>.
    /// </summary>
    /// <remarks>
    /// This is needed to unsure OData URLs work correctly when the application is executing behind a reverse
    /// proxy (e.g. running in docker compose).
    /// 
    /// An example change to the Request URL is: https://localhost:1234/v1/asks -> http://localhost/api/v1/asks
    /// </remarks>
    /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
    /// <param name="webConfiguration">The configuration object containing the API url to map to.</param>
    /// <returns>An <see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseRequestBaseUrlRewriter(this IApplicationBuilder app, WebConfiguration webConfiguration)
    {
        app.Use(async (context, next) =>
        {
            if (string.IsNullOrWhiteSpace(webConfiguration.ApiUrl) == false)
            {
                var apiUri = new Uri(webConfiguration.ApiUrl);
                context.Request.PathBase = new PathString(apiUri.LocalPath);
                context.Request.Host = new HostString(apiUri.Host);
                context.Request.Scheme = apiUri.Scheme;
            }
            await next.Invoke();
        });
        return app;
    }

    /// <summary>
    /// A middleware that validates the OData query and returns the proper error (i.e. 400) if the query is invalid.
    /// </summary>
    /// <remarks>
    /// This is needed because the an ODataException will cause a 500 error to be returned to the client.
    /// </remarks>
    /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
    /// <returns>An <see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseODataExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next.Invoke();
            }
            catch (ODataException odataException)
            {
                var problemDetails = new ProblemDetails()
                {
                    Title = "Bad Request",
                    Instance = context.Request.Path,
                    Status = 400,
                    Detail = odataException.Message,
                };

                if (env.IsDevelopment() || env.IsStaging())
                {
                    problemDetails.Extensions.Add("DeveloperMessage", odataException.ToString());
                }
                context.Response.StatusCode = problemDetails.Status.Value;
                await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(problemDetails));
            }
        });
        return app;
    }

    /// <summary>
    /// Adds OData configuration to the Inklio API.
    /// </summary>
    /// <param name="mvcBuilder">The <see cref="IMvcBuilder"/>.</param>
    /// <returns>An <see cref="IMvcBuilder"/>.</returns>
    public static IMvcBuilder AddApiOData(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddOData(options => 
        {
            options.EnableQueryFeatures();
            options.EnableNoDollarQueryOptions = true;
            options.RouteOptions.EnableKeyAsSegment = true;
            options.RouteOptions.EnableUnqualifiedOperationCall = true;
            options.RouteOptions.EnableActionNameCaseInsensitive = true;
            options.RouteOptions.EnableControllerNameCaseInsensitive = true;
            options.RouteOptions.EnableNonParenthesisForEmptyParameterFunction = true;
            options.RouteOptions.EnablePropertyNameCaseInsensitive = true;
            options.RouteOptions.EnableKeyInParenthesis = true;
            options.UrlKeyDelimiter = ODataUrlKeyDelimiter.Slash;
            options.AddRouteComponents("v1", CreateEdmModel());
        });
        return mvcBuilder;
    }
    
    /// <summary>
    /// Creates the EDM model used to route OData queries.
    /// </summary>
    /// <returns>The EDM model used to route OData queries.</returns>
    private static IEdmModel CreateEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.ModelAliasingEnabled = true;
        builder.EnableLowerCamelCase();
        builder.EntitySet<Ask>("asks").EntityType
            .HasKey(e => e.Id)
            .Page(10, 10);
        builder.EntitySet<Delivery>("deliveries").EntityType
            .HasKey(e => e.Id)
            .Page(20, 20)
            .Expand(2, SelectExpandType.Automatic, "images")
            .Expand(2, SelectExpandType.Automatic, "tags");
        builder.EntitySet<Comment>("comments").EntityType
            .HasKey(e => e.Id)
            .Page(20, 20);
        builder.EntityType<Image>()
            .HasKey(e => e.Id);
        builder.EntityType<Tag>()
            .HasKey(e => e.Id);

        return builder.GetEdmModel();
    }
}