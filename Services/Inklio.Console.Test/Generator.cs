using System.Text.Json;
using Inklio.Api.Client;

public static class Generator
{
    public static async Task GenerateAskDeliveryCommentUpvoteAsync(string filePath)
    {
        var user1 = new User("testuser1");
        var user2 = new User("testuser2");
        await user1.CreateOrLoginAsync();
        await user2.CreateOrLoginAsync();

        // Create an Ask
        var getOrCreateAsksAsync = async () =>
        {
            IEnumerable<Ask> asks = (await user2.GetAsksAsync()).Value;
            if (asks.Any() == false)
            {
                await user2.AddAskAsync(new AskCreate()
                {
                    Body = "My Ask Body",
                    ContentRating = 1,
                    Title = "My Ask Title",
                    Images = new byte[][] { File.ReadAllBytes(filePath), File.ReadAllBytes(filePath) },
                    Tags = new Tag[] { new Tag("aqua") }
                });
            }
            var newAsks = await user2.GetAsksAsync();
            return newAsks.Value;
        };
        var asks = await getOrCreateAsksAsync();

        // Create a Delivery
        if (asks.First().Deliveries.Any() == false)
        {
            await user1.AddDeliveryAsync(new DeliveryCreate()
            {
                Body = "My Delivery Body",
                ContentRating = 1,
                Title = "My Delivery Title",
                IsSpoiler = true,
                Images = new byte[][] { File.ReadAllBytes(filePath), File.ReadAllBytes(filePath) },
                Tags = new Tag[] { new Tag("aqua") }

            },
            asks.First().Id);
        }

        asks = await getOrCreateAsksAsync();
        if (asks.First().Comments.Any() == false)
        {
            await user1.AddCommentAsync("My Ask Comment", asks.First().Id);
        }

        asks = await getOrCreateAsksAsync();
        if (asks.First().Deliveries.First().Comments.Any() == false)
        {
            await user1.AddCommentAsync("My Delivery Comment", asks.First().Id, asks.First().Deliveries.First().Id);
        }

        // Add upvotes
        foreach (var ask in asks)
        {
            await user1.AddUpvoteAsync(ask.Id);
            await user2.AddUpvoteAsync(ask.Id);

            foreach (var d in ask.Deliveries)
            {
                await user1.AddUpvoteAsync(ask.Id, d.Id);
                await user2.AddUpvoteAsync(ask.Id, d.Id);
            }
        }

        var finalResults = await user1.GetAsksAsync();
        string json = JsonSerializer.Serialize(finalResults);
        Console.WriteLine(json);
    }
}