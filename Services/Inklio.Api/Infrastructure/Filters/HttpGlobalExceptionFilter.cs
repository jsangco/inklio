using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.ActionResults;
using Microsoft.OData;
using Microsoft.VisualBasic;

namespace Inklio.Api.Infrastructure.Filters;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    private readonly IWebHostEnvironment env;
    private readonly ILogger<HttpGlobalExceptionFilter> logger;

    public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
    {
        this.env = env;
        this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        logger.LogError(new EventId(context.Exception.HResult),
            context.Exception,
            context.Exception.Message);

        if (context.Exception is InklioDomainException inklioException)
        {
            var problemDetails = new ProblemDetails()
            {
                Title = inklioException.RecommendedStatusCode is 401 ? "Unauthorized." :
                    inklioException.RecommendedStatusCode is 403 ? "Forbidden." :
                    "Invalid Request.",
                Instance = context.HttpContext.Request.Path,
                Status = inklioException.RecommendedStatusCode,
                Detail = inklioException.Message.ToString(),
            };

            if (inklioException.Errors is not null)
            {
                problemDetails.Extensions.Add("errors", inklioException.Errors);
            }

            context.Result = new ObjectResult(problemDetails) { StatusCode = inklioException.RecommendedStatusCode };
            context.HttpContext.Response.StatusCode = inklioException.RecommendedStatusCode;
        }
        else if (context.Exception is ODataException odataException)
        {
            var problemDetails = new ProblemDetails()
            {
                Title = "Bad Request",
                Instance = context.HttpContext.Request.Path,
                Status = 400,
                Detail = odataException.Message,
            };

            if (env.IsDevelopment() || env.IsStaging())
            {
                problemDetails.Extensions.Add("DeveloperMessage", context.Exception.ToString());
            }
            context.Result = new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
            context.HttpContext.Response.StatusCode = problemDetails.Status.Value;
        }
        else
        {
            var problemDetails = new ProblemDetails()
            {
                Title = "Internal Server Error.",
                Detail = "An unexpected error occured. Please try again later.",
                Status = 500,
            };

            if (env.IsDevelopment() || env.IsStaging())
            {
                problemDetails.Extensions.Add("DeveloperMessage", context.Exception.ToString());
            }

            // Result asigned to a result object but in destiny the response is empty. This is a known bug of .net core 1.1
            // It will be fixed in .net core 1.1.2. See https://github.com/aspnet/Mvc/issues/5594 for more information
            context.Result = new InternalServerErrorObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        context.ExceptionHandled = true;
    }
}
