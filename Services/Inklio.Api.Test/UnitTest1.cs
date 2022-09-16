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
        var configuration = new ConfigurationBuilder().Build();
        ContainerBuilder builder = new ContainerBuilder();
        builder.RegisterModule(new InklioDependencyModule(configuration, new TestHost()));
        IContainer container = builder.Build();
        using (var scope = container.BeginLifetimeScope())
        {
            var context = scope.Resolve<InklioContext>();

            var ask = new Ask("myAskBody", 0, true, true, "myAskTitle");
            ask.AddComment("myAskCommentBody", 0);
            var delivery = ask.AddDelivery("myDeliveryBody", 0, true, true, "myDeliveryTitle");
            delivery.AddComment("myDeliveryCommentBody", 0);

            context.Asks.Add(ask);

            context.SaveChanges();
        }
        using (var scope = container.BeginLifetimeScope())
        {
            var context = scope.Resolve<InklioContext>();
            var asks = context.Asks.ToArray();
            var comments = context.Comments.ToArray();
            var askComments = context.AskComments.ToArray();
            var deliveries = context.Deliveries.ToArray();
            var test = JsonSerializer.Serialize(asks.First(), new JsonSerializerOptions(){ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles});
            Console.WriteLine(test);
            Console.WriteLine(asks.Length);
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