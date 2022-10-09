namespace Inklio.Api.Infrastructure.Filters;

public class JsonErrorResponse
{
    public string[] Messages { get; set; } = new string[] { };

    public string DeveloperMessage { get; set; } = string.Empty;
}

