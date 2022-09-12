namespace Inklio.Api.Infrastructure.Filters;

public class ValidateModelStateFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

#pragma warning disable CS8602
        var validationErrors = context.ModelState
            .Keys
            .SelectMany(k => context.ModelState[k].Errors)
            .Select(e => e.ErrorMessage)
            .ToArray();
#pragma warning restore CS8602


        var json = new JsonErrorResponse
        {
            Messages = validationErrors
        };

        context.Result = new BadRequestObjectResult(json);
    }
}

