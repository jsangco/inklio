using System.Text.Json;
using Autofac;
using Azure.Core;
using Inklio.Api.Application.Commands.Accounts;
using Inklio.Api.Dependencies;
using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Inklio.Api.Client;

namespace Inklio.Api.Test;

[Trait("api", "integration")]
public class ContentSeedingTest
{

    // public static User[] Users = Enumerable.Range(0, Usernames.Length).Select(i => new User(Usernames[i], Emails[i])).ToArray();

    [Fact]
    public void SeedContent()
    {
        // Create 100 users
        // Randomly create 100 asks from 100 different users
        // Randomly create 100 delivers from 100 different users
        // Randomly create 200 ask comments from 100 different users
        // Randomly create 200 delivery comments from 100 different users
        // Randomly uptvote asks from 100 different users
        // Randomly uptvote deliveries from 100 different users
        // Randomly uptvote asks comments from 100 different users
        // Randomly uptvote deliveries comments from 100 different users
        // Store user actions locally to prevent duplicate
        // user.AddAsk(new AskCreateCommand());
    }
}