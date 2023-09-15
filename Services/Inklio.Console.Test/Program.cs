using System.Text.Json;
using Inklio.Api.Client;

Console.WriteLine("Starting program");

User.DefaultBaseUri = new Uri("https://localhost:7187");

var contentGenerator = new ContentGenerator(ContentGenerator.Usernames);
await contentGenerator.LoginAllUsersAsync();
await contentGenerator.CreateAsksAsync();
await contentGenerator.CreateDeliveriesAsync("C:\\src\\Inklio\\aqua.png");
await contentGenerator.CreateAskComments();
await contentGenerator.CreateDeliveryComments();