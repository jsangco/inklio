# Introduction
Code Repo for the Inklio project - A community for creatives to get inspirational prompts.

## Running locally

All services can be run together using docker compose. The configuration steps are provided below.

### 1. Install development Tools

* [VSCode](https://code.visualstudio.com/) - IDE
  * [SQL Server (mssql) Extension](https://github.com/microsoft/vscode-mssql)
* [.Net 6+ SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) - For building C# projects
* [Docker](https://docs.docker.com/get-docker/) - For building and running containers

### 2. SQL Database connection

> NOTE: Local SQL debugging can be used by running `docker compose up`

1. [Grant your IP address access](https://learn.microsoft.com/en-us/azure/azure-sql/database/network-access-controls-overview?view=azuresql#allow-azure-services) to the SQL DB in the Azure Portal
2. Retrieve the connection string from the Azure Portal
3. In the VS Code SQL Server Extension add a connection to the DB 

### 3. Configure environment secrets

For execution using local docker images, simply rename the [.env.local](./.env.local) file to `.env`.

The `.env` file can also be modified to use remote resources.

```
  SQLAZURECONNSTR_InklioSqlConnectionString=<sql connection string here>
  CONNECTIONSTRINGS__InklioStorageConnectionString=<azure storage connection string here>
```

> **NOTE:** The connection strings should not be surrounded by quotes

> **WARNING:** The DNS name in the **azure storage connection string**'s URL *must* be configured to match the environment the application is running in. If the application is running within docker compose, the URL should be "http://**azure-storage**/:10000/devstoreaccount1" if the application is running outside of docker on the local machine, the URL should be "http://**127.0.0.1**:10000/devstoreaccount1;" This is because of the way [docker networking](https://docs.docker.com/compose/networking/) works.
>
> When running docker compose, use the values from [.env.local](./.env.local).

### 4. Build Release and run Docker Compose

Run `dotnet build -c Release .\Inklio.sln` from the [./Services/Inklio.Api](./Services/Inklio.Api) directory.

Run `yarn run build` from the [./Client/Web/inklio](./Client/Web/inklio) directory.

Run `docker compose up --build` from the [docker-compose.yml](./docker-compose.yml) (i.e. the root) file directory. If the application starts correctly you should be able to run `curl http://localhost`

> **NOTE:** The HTTPS connection does not work because there is no local SSL cert--use HTTP.

> **NOTE:** If changes are made to a service, you must rebuild the `Release` version of the service and then run `docker compose up --build`. If this is not done, the new changes will not be included.

### 5. Seed content or Test the API

Seeding content can be done by running the [Inklio.Console.Test](Services\Inklio.Console.Test) application.

```
cd ./Services/Inklio.Console.Test
dotnet run
```

This will generate a bunch of psuedo-content on the locally running website.

#### Testing the APIs manually.

The swagger page for the APIs can be accessed using the following URL [http://localhost:80/api/swagger/](http://localhost:80/api/swagger/)

Various CLI commands are provided below that can be used to test the API from the CLI

Below are some sample powershell commands that can be used to interact with the Inklio.Api endpoints.

### Account Creation

``` powershell
$accountCreate = '{"email":"inkliojace@mailinator.com","username":"jace","password":"aoeuaoeu1","confirmPassword":"aoeuaoeu1"}'
Invoke-WebRequest http://localhost/api/v1/accounts/register -Method POST -ContentType "application/json" -Body $accountCreate

$accountLogin = '{"username":"jace","password":"aoeuaoeu1","isRememberMe":false}'
Invoke-WebRequest http://localhost/api/v1/accounts/login -Method POST -ContentType "application/json" -Body $accountLogin

$accountForget = '{"email":"inkliojace@mailinator.com"}'
Invoke-WebRequest http://localhost/api/v1/accounts/forget -Method POST -ContentType "application/json" -Body $accountForget

$accountReset = '{"email":"inkliojace@mailinator.com","password":"aoeuaoeu1","confirmPassword":"aoeuaoeu1","code":"REPLACE_RESET_CODE_HERE"}'
Invoke-WebRequest http://localhost/api/v1/accounts/reset -Method POST -ContentType "application/json" -Body $accountReset
```

### Application

> **NOTE:** These API calls will fail without proper authentication.

```powershell

# Create an Ask
$askCreateCommand = @{"ask"="{'body':'my body'}"; images=(get-item -path ./aqua.png)}
Invoke-WebRequest -Method POST -Form $askCreateCommand -ContentType "multipart/form-data" https://localhost/api/v1/asks

# Add a Delivery to an Ask
$deliveryCreateCommand = @{"body"="myDeliveryBodyPs"; "title"="myDeliveryTitlePs";"isNsfw"=$true;"isNsfl"=$false;IsNsfw=$true; images=(get-item -path ./aqua.png)}
Invoke-WebRequest -Method POST -Form $deliveryCreateCommand -ContentType "multipart/form-data" https://localhost/api/v1/asks/1/deliveries

# Add a Comment to an Ask
Invoke-WebRequest -Method POST -Body (@{"body"="myAskComment";} | ConvertTo-Json) -ContentType "application/json" https://localhost/api/v1/asks/1/comments

# Add a Comment to a Delivery
Invoke-WebRequest -Method POST -Body (@{"body"="myDeliveryComment";} | ConvertTo-Json) -ContentType "application/json" https://localhost/api/v1/asks/1/deliveries/1/comments

# Add a Tag to an Ask and all its child objects (i.e. Deliveries, Comments)
Invoke-WebRequest -Method POST -Body (@{"tag"=@{"value"="konosuba"}} | ConvertTo-Json)  -ContentType "application/json" https://localhost/api/v1/asks/1/tags

# Get all Asks
curl http://localhost:80/api/v1/asks

# Get all Asks but include their Deliveries, Delivery Comments, and Ask Comments. (This done with OData)
curl "http://localhost:80/api/v1/asks?expand=deliveries(expand=comments,images),comments,images"

# Get all the Deliveries from the first Ask
curl http://localhost:80/api/v1/asks/1/deliveries
```