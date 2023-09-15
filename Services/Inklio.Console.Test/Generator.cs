using System.Text.Json;
using Inklio.Api.Client;

public static class Generator
{
    public static async Task GenerateAskDeliveryCommentAsync()
    {
        var user = new User("jace5");
        await user.CreateOrLoginAsync();

        var getOrCreateAsksAsync = async () =>
        {
            var askCreator = new User("jace4");
            IEnumerable<Ask> asks = (await askCreator.GetAsksAsync()).Value;
            if (asks.Any() == false)
            {
                await askCreator.AddAskAsync(new AskCreate()
                {
                    Body = "My Ask Body",
                    Title = "My Ask Title",
                    IsNsfl = true,
                    IsNsfw = false,
                    Images = new byte[][] { File.ReadAllBytes("C:\\src\\Inklio\\aqua.png"), File.ReadAllBytes("C:\\src\\Inklio\\aqua.png") },
                    Tags = new Tag[] { new Tag("aqua") }
                });
            }
            var newAsks = await askCreator.GetAsksAsync();
            return newAsks.Value;
        };

        var asks = await getOrCreateAsksAsync();
        if (asks.First().Deliveries.Any() == false)
        {
            await user.AddDeliveryAsync(new DeliveryCreate()
            {
                Body = "My Delivery Body",
                Title = "My Delivery Title",
                IsNsfl = true,
                IsNsfw = false,
                IsSpoiler = true,
                Images = new byte[][] { File.ReadAllBytes("C:\\src\\Inklio\\aqua.png"), File.ReadAllBytes("C:\\src\\Inklio\\aqua.png") },
                Tags = new Tag[] { new Tag("aqua") }

            },
            asks.First().Id);
        }

        if (asks.First().Comments.Any() == false)
        {
            await user.AddCommentAsync("My Ask Comment", asks.First().Id);
        }

        if (asks.First().Deliveries.First().Comments.Any() == false)
        {
            await user.AddCommentAsync("My Delivery Comment", asks.First().Id, asks.First().Deliveries.First().Id);
        }

        var finalResults = await user.GetAsksAsync();
        string json = JsonSerializer.Serialize(finalResults);
        Console.WriteLine(json);
    }
}