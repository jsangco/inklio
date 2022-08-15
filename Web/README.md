# Introduction
The Front-end web projects for Inklio

## Recommended Development Tools

* [VSCode](https://code.visualstudio.com/) - IDE
  * [SQL Server (mssql) Extension](https://github.com/microsoft/vscode-mssql)
* [.Net 6+ SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) - For building C# projects
* [Docker](https://docs.docker.com/get-docker/) - For building and running containers

## SQL Database connection

1. Grant your IP address access to the SQL DB in the Azure Portal
2. Retrieve the connection string from the Azure Portal
3. In the SQL Server Extension add a connection to the DB

## Local App configuration

To run te API on your local machine, you must set all Secrets and Connection Strings in the `appsettings.json`. These secrets should **not** be set in the `appsettings.json`, but should instead be set as environment variables.

The environment variables only need to be set when the application is running, so the easiest way to set them is by configuring the `env` values in the `.vscode/launch.json` that is automatically created by VSCode when trying to run/debug the app locally.

Here is an example.

```
  "env": {
      "ASPNETCORE_ENVIRONMENT": "Development",
      "SQLAZURECONNSTR_InklioSqlConnectionString":"<MyConnectionString>"
  },
```

These values are obviously secret, so please find the values in the azure portal or contact the application owner.
