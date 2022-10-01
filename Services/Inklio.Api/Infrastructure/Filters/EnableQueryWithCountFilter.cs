using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;

namespace Inklio.Api.Infrastructure.Filters
{
    public partial class EnableQueryWithCountAttribute : EnableQueryAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            HttpContext context = actionExecutedContext.HttpContext;

            // NOTE: TotalCount will be null if
            // 1) Count is not enabled during startup;
            // 2) if $count=true is not used in the url.

            long? count = context.Request.ODataFeature().TotalCount;
            if (count.HasValue)
            {
                context.Response.Headers.Add("X-Total-Count", count.Value.ToString());
            }
        }
    }
}
