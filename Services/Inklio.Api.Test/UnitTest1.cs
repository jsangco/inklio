using System.Text.Json;
using Autofac;
using Inklio.Api.Dependencies;
using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Inklio.Api.Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Environment.SetEnvironmentVariable("Web__BaseUrl", "http://localhost/");
        Environment.SetEnvironmentVariable("Web__ApiUrl", "http://localhost/api/");
        Environment.SetEnvironmentVariable("Web__ImageUrl", "http://localhost/images/");

        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
        ContainerBuilder builder = new ContainerBuilder();
        builder.RegisterModule(new InklioDependencyModule(configuration, new TestHost()));
        IContainer container = builder.Build();
        using (var scope = container.BeginLifetimeScope())
        {
            var context = scope.Resolve<InklioContext>();
        }
    }

    public class TestHost : IHostEnvironment
    {
        public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EnvironmentName { get; set; } = "Development";
    }
}