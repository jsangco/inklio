using Autofac;
using Inklio.Api.Dependencies;
using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting program");

var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
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
        context.Tags.Add(Tag.Create(user, "testType", "testValue"));
        context.SaveChanges();
    }
    if (context.Asks.Count() < 2)
    {
        var ask = Ask.Create("myAskBody4", user, true, true, "myAskTitle3");
        ask.AddDelivery("myDeliveryBody", user, false, true, "myDeliveryTitle");
        ask.AddComment("myCommentBody", user);
        context.Asks.Add(ask);
        context.SaveChanges();
    }
}

using (var scope = container.BeginLifetimeScope())
{
    var context = scope.Resolve<InklioContext>();

    var user = context.Users.First();
    var ask = context.Asks
        .Include(e => e.Tags)
        .Include(e => e.Upvotes)
        .Include(e => e.Flags)
        .Include(e => e.Comments).ThenInclude(e => e.Flags)
        .Include(e => e.Comments).ThenInclude(e => e.Upvotes)
        .Include(e => e.Deliveries).ThenInclude(e => e.Tags)
        .Include(e => e.Deliveries).ThenInclude(e => e.Upvotes)
        .Include(e => e.Deliveries).ThenInclude(e => e.Flags)
        .ToList().Last();

    ask.AddFlag(FlagType.Spam, user);
    ask.Deliveries.First().AddUpvote(0, user);
    ask.Comments.First().AddUpvote(0, user);
    ask.Deliveries.First().AddFlag(FlagType.Harassment, user);
    ask.Comments.First().AddFlag(FlagType.ThreateningViolence, user);

    Console.WriteLine(context.ChangeTracker.DebugView.LongView);
    Console.WriteLine("");
    Console.WriteLine(context.ChangeTracker.DebugView.ShortView);
    context.SaveChanges();
}

using (var scope = container.BeginLifetimeScope())
{
    var context = scope.Resolve<InklioContext>();
    var users = context.Users.ToArray();
    var asks = context.Asks
        .Include(e => e.Tags)
        .Include(e => e.Upvotes)
        .Include(e => e.Flags)
        .Include(e => e.Comments).ThenInclude(e => e.Flags)
        .Include(e => e.Comments).ThenInclude(e => e.Upvotes)
        .Include(e => e.Deliveries).ThenInclude(e => e.Tags)
        .Include(e => e.Deliveries).ThenInclude(e => e.Upvotes)
        .Include(e => e.Deliveries).ThenInclude(e => e.Flags)
        .ToList();
    var tags = context.Tags.Include(e => e.Asks).ToArray();
    // var askTags = context.AskTags.ToArray();
    var comments = context.Comments.ToArray();
    var askComments = context.AskComments.ToArray();
    var deliveries = context.Deliveries.Include(e => e.Tags).Include(e => e.Upvotes).ToArray();
}