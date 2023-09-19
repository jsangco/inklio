using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Inklio.Api.Client;

Console.WriteLine("Starting program");

await Generator.GenerateAskDeliveryCommentAsync();

var contentGenerator = new ContentGenerator(ContentGenerator.Usernames);
await contentGenerator.SeedContent("C:\\src\\Inklio\\aqua.png");