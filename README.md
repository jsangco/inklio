# Introduction
Code Repo for the Inklio project

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

## Docker

Docker can be used to run a local environment of the Inklio services.

### Running Docker on a single image

The following steps can be used to run Docker for a single service

1. `cd myServiceDirectory`
2. `dotnet publish myservices.csproj -c Release`
3. `docker build . -t myservice:latest`
4. `docker run -it --rm -p 8765:80 -p 8001:443 myservice:latest`
5. `curl http://localhost:8765`

> NOTE: the `-p` tag must become before the container name or things don't work

### Running all services with Docker compose

The following steps can be used to run all services in docker 

1. Build and tag every docker image
2. From the `docker-compose.yml` file director run: `docker compose up`
3. `curl http://localhost:8765`

> NOTE: The HTTPS connection does not work because there is no local SSL cert
