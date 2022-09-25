using Inklio.Api.Domain;
// using Inklio.Api.Application.Commands;
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
        var ask = builder.EntitySet<Ask>("Asks");
        ask.EntityType.Name = "Ask";
        builder.EntityType<Ask>()
            .HasKey(e => e.Id);
        var delivery = builder.EntitySet<Delivery>("Deliveries");
        delivery.EntityType.Name = "Delivery";
        builder.EntityType<Delivery>()
            .HasKey(e => e.Id);
        var comment = builder.EntitySet<Comment>("Comments");
        comment.EntityType.Name = "Comment";
        builder.EntityType<Comment>()
            .HasKey(e => e.Id);


        return builder.GetEdmModel();
    }
}