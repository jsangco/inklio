# Inklio C# API Client library

This is the auto-generated cilent for the Inklio APIs.

## Client API generation

### Prerequisites

1. Must be able to build and run the [Inklio.Api](../../Services/Inklio.Api/README.md) project locally.
2. [Node.js](https://nodejs.org/en)
3. Autorest (`npm install -g autorest`)

### Steps

1. Build and run the Inklio API server locally.
2. From the [Inklio.Api.Client](.) directory, run `curl https://localhost:7187/swagger/v1/swagger.json` to update the [swagger.json](swagger.json) file.
3. Run `autorest --csharp --input-file=.\swagger.json --namespace="Inklio.Api.Client"`
4. Update the [Inklio.Api.Client.csproj](Inklio.Api.Client.csproj) by changing the `TargetFramework` property to net6.0 and specify the version of `Azure.Core` (e.g. `Version="1.35.0"`).
