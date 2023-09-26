using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Inklio.Api.Client;

Console.WriteLine("Starting program");

string sampleFilePath = "..\\..\\aqua.png";
User.DefaultBaseUri = new Uri("http://localhost/api/");

await Generator.GenerateAskDeliveryCommentUpvoteAsync(sampleFilePath);

var contentGenerator = new ContentGenerator(ContentGenerator.Usernames);
await contentGenerator.SeedContent(sampleFilePath);