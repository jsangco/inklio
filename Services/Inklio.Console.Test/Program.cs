using Autofac;
using Inklio.Api.Dependencies;
using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting program");

var configuration = new ConfigurationBuilder().Build();
ContainerBuilder builder = new ContainerBuilder();
builder.RegisterModule(new InklioDependencyModule(configuration, new TestHost()));
IContainer container = builder.Build();

using (var scope = container.BeginLifetimeScope())
{
    var context = scope.Resolve<InklioContext>();

    if (context.Users.Count() < 1)
    {
        context.Users.Add(new User("testusername"));
        context.SaveChanges();
    }
    var user = context.Users.First();
    if (context.Tags.Count() < 1)
    {
        context.Tags.Add(new Tag(user, "testType", "testValue"));
        context.SaveChanges();
    }
    if (context.Asks.Count() < 8)
    {
        var ask = new Ask("myAskBody4", user, true, true, "myAskTitle3");
        ask.AddDelivery("myDeliveryBody", user, false, true, "myDeliveryTitle");
        context.Asks.Add(ask);
        context.SaveChanges();
    }
}

using (var scope = container.BeginLifetimeScope())
{
    var context = scope.Resolve<InklioContext>();

    var user = context.Users.First();
    var ask = context.Asks.Include(e => e.Tags).Include(e => e.Deliveries).ThenInclude(e => e.Tags).ToList().Last();
    var tag = context.Tags.First();// ?? throw new InvalidOperationException();
    var delivery = ask.Deliveries.First();
    delivery.AddTag(user, tag);
    // var ask = context.Asks.Where(e => e.Id > 1).Include(e => e.Tags).Include(e => e.AskTags).First();
    // var tag = context.Tags.Include(e => e.Asks).Include(e => e.AskTags).First();

    ask.AddTag(user, tag);
    // var at = new AskTag(ask, user, tag);
    // context.Add(at);
    // ask.AskTags.Add(at);
    // ask.Tags.Add(tag);
    // tag.Asks.Add(ask);
    // tag.AskTags.Add(at);

    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    Console.WriteLine("");
    Console.WriteLine(context.ChangeTracker.DebugView.ShortView);
    context.SaveChanges();
}

using (var scope = container.BeginLifetimeScope())
{
    var context = scope.Resolve<InklioContext>();
    var users = context.Users.ToArray();
    var asks = context.Asks.Include(e => e.Tags).ToArray();
    var tags = context.Tags.Include(e => e.Asks).ToArray();
    // var askTags = context.AskTags.ToArray();
    var comments = context.Comments.ToArray();
    var askComments = context.AskComments.ToArray();
    var deliveries = context.Deliveries.Include(e => e.Tags).ToArray();
}

//     // ask.AddComment("myAskCommentBody", user);
//     // var delivery = ask.AddDelivery("myDeliveryBody", user, true, true, "myDeliveryTitle");
//     // delivery.AddComment("myDeliveryCommentBody", user);
// }
// using (var scope = container.BeginLifetimeScope())
// {
//     var context = scope.Resolve<InklioContext>();

//     var tagos = context.Tagos.First();
//     var post = context.Posts.First();

//     var user = context.Users.First();
//     var ask = context.Asks.First();
//     var tag = context.Tags.First();
//     ask.AddTag(user, tag);
//     context.SaveChanges();
// }
// using (var scope = container.BeginLifetimeScope())
// {
//     var context = scope.Resolve<InklioContext>();
//     var users = context.Users.ToArray();
//     var tags = context.Tags.ToArray();
//     var asks = context.Asks.ToArray();
//     // var askTags = context.AskTags.ToArray();
//     var comments = context.Comments.ToArray();
//     var askComments = context.AskComments.ToArray();
//     var deliveries = context.Deliveries.ToArray();
//     var test = JsonSerializer.Serialize(asks.First(), new JsonSerializerOptions(){ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles});
//     Console.WriteLine(test);
//     Console.WriteLine(asks.Length);
// }