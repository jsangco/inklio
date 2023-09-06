using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.ActionResults;
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

        if (context.Exception is InklioDomainException exception)
        {
            var problemDetails = new ProblemDetails()
            {
                Title = exception.RecommendedStatusCode is 401 ? "Unauthorized." :
                    exception.RecommendedStatusCode is 403 ? "Forbidden." :
                    "Invalid Request.",
                Instance = context.HttpContext.Request.Path,
                Status = exception.RecommendedStatusCode,
                Detail = exception.Message.ToString(),
            };

            if (exception.Errors is not null)
            {
                problemDetails.Extensions.Add("errors", exception.Errors);
            }

            context.Result = new ObjectResult(problemDetails) { StatusCode = exception.RecommendedStatusCode };
            context.HttpContext.Response.StatusCode = exception.RecommendedStatusCode;
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
