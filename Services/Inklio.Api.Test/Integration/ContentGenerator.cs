using System.Text.Json;
using Autofac;
using Azure.Core;
using Inklio.Api.Application.Commands.Accounts;
using Inklio.Api.Client;
using Inklio.Api.Dependencies;
using Inklio.Api.Domain;
using Inklio.Api.Infrastructure.EFCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

namespace Inklio.Api.Test;

public class ContentGenerator
{
    public ContentGenerator()
    {

    }

    public void CreateAccount(bool expectSuccess = false)
    {
    }
}