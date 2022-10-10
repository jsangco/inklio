// using Inklio.Api.Domain;
using Inklio.Api.Application.Commands;
using Microsoft.AspNetCore.OData;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Inklio.Api.Startup;

public static class ODataExtensions
{
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
    
    private static IEdmModel CreateEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.ModelAliasingEnabled = true;
        builder.EnableLowerCamelCase();
        builder.EntitySet<Ask>("asks")
            .EntityType
            .HasKey(e => e.Id)
            .Expand(2, SelectExpandType.Automatic, "tags");
        builder.EntitySet<Delivery>("deliveries")
            .EntityType
            .HasKey(e => e.Id)
            .Expand(2, SelectExpandType.Automatic, "images")
            .Expand(2, SelectExpandType.Automatic, "tags");
        builder.EntitySet<Comment>("comments");
        builder.EntityType<Comment>()
            .HasKey(e => e.Id);
        var foo = builder.EntityType<Image>()
            .HasKey(e => e.Url);

        return builder.GetEdmModel();
    }
}