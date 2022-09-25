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
            options.AddRouteComponents("v1", CreateEdmModel());
            options.EnableQueryFeatures();
            options.EnableNoDollarQueryOptions = true;
            options.RouteOptions.EnableActionNameCaseInsensitive = true;
            options.RouteOptions.EnableControllerNameCaseInsensitive = true;
            options.RouteOptions.EnableNonParenthesisForEmptyParameterFunction = true;
            options.RouteOptions.EnablePropertyNameCaseInsensitive = true;
            options.UrlKeyDelimiter = ODataUrlKeyDelimiter.Slash;
        });
        return mvcBuilder;
    }
    
    private static IEdmModel CreateEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EnableLowerCamelCase();
        builder.EntitySet<Ask>("Asks");
        builder.EntityType<Ask>()
            .HasKey(e => e.Id);
        builder.EntitySet<Delivery>("Deliveries");
        builder.EntityType<Delivery>()
            .HasKey(e => e.Id);
        builder.EntitySet<Comment>("Comments");
        builder.EntityType<Comment>()
            .HasKey(e => e.Id);


        return builder.GetEdmModel();
    }
}