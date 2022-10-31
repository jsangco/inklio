# Introduction
Code Repo for the Inklio project

## Running locally

All services can be run together using docker compose. The configuration steps are provided below.

### 1. Install development Tools

* [VSCode](https://code.visualstudio.com/) - IDE
  * [SQL Server (mssql) Extension](https://github.com/microsoft/vscode-mssql)
* [.Net 6+ SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) - For building C# projects
* [Docker](https://docs.docker.com/get-docker/) - For building and running containers

### 2. SQL Database connection

1. [Grant your IP address access](https://learn.microsoft.com/en-us/azure/azure-sql/database/network-access-controls-overview?view=azuresql#allow-azure-services) to the SQL DB in the Azure Portal
2. Retrieve the connection string from the Azure Portal
3. In the VS Code SQL Server Extension add a connection to the DB 

### 3. Configure environment secrets

After receiving the application secrets from an administrator, from the `docker-compose.yml` file directory, create a `.env` file and add the following values:
```
  SQLAZURECONNSTR_InklioSqlConnectionString=<sql connection string here>
  CONNECTIONSTRINGS__InklioStorageConnectionString=<azure storage connection string here>
```
> **NOTE:** The connection strings should not be surrounded by quotes

### 4. Build Docker containers

A docker container must be built and tagged for every application. The docker build steps are provided in the application's README file:
1. [Inklio.Api](services/Inklio.Api/README.md)

### Running all services with Docker compose

After building and tagging every application run `docker compose up` from the `docker-compose.yml` file directory. If the application starts correctly you should be able to run `curl http://localhost:80`

> **NOTE:** The HTTPS connection does not work because there is no local SSL cert; use HTTP.
