namespace Inklio.Api.Infrastructure.Filters;

public class JsonErrorResponse
{
    public string[] Messages { get; set; } = new string[] { };

    public object DeveloperMessage { get; set; } = new object();
}

