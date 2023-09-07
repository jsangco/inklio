using System.Text.Json;
using Autofac;
using Inklio.Api.Dependencies;
using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

namespace Inklio.Api.Test;

[Trait("api", "integration")]
public class ContentSeeding
{
    [Fact]
    public void SeedContent()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        string baseUrl = configuration.GetValue<string>("inklioUrl") ?? "http://localhost";
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(baseUrl);

        // httpClient.PostAsync(new HttpRequestMessage(HttpMethod.Get, rea))
    }
}