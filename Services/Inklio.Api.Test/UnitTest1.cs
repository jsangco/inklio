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

            var ask = new Ask() { Body = "myAsk", CanComment = true, UpvoteCount = 123};
            var ask2 = new Ask() { Body = "myAsk2", CanComment = true, UpvoteCount = 456};
            
            // var delivery = new Delivery() { Body = "myDelivery" };
            // ask.AddDelivery(delivery);
            
            // var askComment = new AskComment() { Body = "myAskComment" };
            // ask.AddComment(askComment);

            // var deliveryComment = new DeliveryComment() { Body = "myDeliveryComment" };
            // delivery.AddComment(deliveryComment);

            context.Asks.Add(ask);
            context.Asks.Add(ask);
            context.Asks.Add(ask2);

            context.SaveChanges();
        }
        using (var scope = container.BeginLifetimeScope())
        {
            var context = scope.Resolve<InklioContext>();
            var asks = context.Asks.ToArray();
            // var comments = context.Comments.ToArray();
            // var askComments = context.AskComments.ToArray();
            // var deliveries = context.Deliveries.ToArray();
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