# Inklio.API
The Inklio API service contains all the APIs and domain logic used to power Inklio.

## Running locally

### 1. Install development tools

* [VSCode](https://code.visualstudio.com/) - IDE
  * [SQL Server (mssql) Extension](https://github.com/microsoft/vscode-mssql)
* [.Net 6+ SDK](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) - For building C# projects
* [Docker](https://docs.docker.com/get-docker/) - For building and running containers

### 2. SQL Database connection

1. [Grant your IP address access](https://learn.microsoft.com/en-us/azure/azure-sql/database/network-access-controls-overview?view=azuresql#allow-azure-services) to the SQL DB in the Azure Portal
2. Retrieve the connection string from the Azure Portal
3. In the VS Code SQL Server Extension add a connection to the DB 

### 3. Configure environment secrets

Secrets and Connection Strings are listed in the `appsettings.json`. These secrets should **not** be set in the `appsettings.json`, but should instead be set as environment variables.

The environment variables only need to be set when the application is running, so the easiest way to set them is by configuring the `env` values in the `.vscode/launch.json` that is automatically created by VSCode when trying to run/debug the app locally.

Here is an example.

```
  "env": {
      "ASPNETCORE_ENVIRONMENT": "Development",
      "SQLAZURECONNSTR_InklioSqlConnectionString":"<MyConnectionString>"
  },
```

These values are obviously secret, so please find the values in the azure portal or contact the application owner.

### 4. Bulid and run the code

Building and running the code can be done from the cmd line 
```powershell
cd /Services/Inklio.Api
dotnet build
dotnet run
```

Information on debugging code can be found [here](https://code.visualstudio.com/docs/editor/debugging).

## Using Docker

### Building the dockerfile

1. `cd /Services/Inklio.Api`
2. `dotnet publish -c Release`
3. `docker build . -t inklio.azurecr.io/docker/inklio.api:latest`
4. Create a `.env` file and add the following values:
```
  SQLAZURECONNSTR_InklioSqlConnectionString=<sql connection string here>
  CONNECTIONSTRINGS__InklioStorageConnectionString=<azure storage connection string here>
```
> NOTE: The connection strings should not be surrounded by quotes; it won't work.
5. `docker run -it --rm -p 8765:80 -p 8001:443 --env-file .\.env inklio.azurecr.io/docker/inklio.api:latest`
6. `curl http://localhost:8765`

### Running the docker container

The following steps can be used to run Docker for a single service

1. `cd /Services/Inklio.Api`
2. `dotnet publish -c Release`
3. `docker build . -t inklio.azurecr.io/docker/inklio.api:latest`
4. `docker run -it --rm -p 8765:5000 -p 8001:443 inklio.azurecr.io/docker/inklio.api:latest`
5. `curl http://localhost:8765`

> NOTE: The `-p` tag must become before the container name or things don't work.
> NOTE: Additional build steps may be described in a [README.md](./inklio.api/../README.md) located in the tareget service's directory.


## Testing APIs from Powershell

The swagger page for the APIs can be accessed using the following URL [http://localhost:80/api/swagger/](http://localhost:80/api/swagger/)

Various CLI commands are provided below that can be used to test the API from the CLI

Below are some sample powershell commands that can be used to interact with the Inklio.Api endpoints

### Account Creation

``` powershell
$accountCreate = '{"email":"inkliojace@mailinator.com","username":"jace","password":"aoeuaoeu1","confirm_password":"aoeuaoeu1"}'
Invoke-WebRequest -Method POST -Body $accountCreate -ContentType "application/json" https://localhost:7187/v1/accounts/register

$accountLogin = '{"username":"jace","password":"aoeuaoeu1","is_remember_me":false}'
Invoke-WebRequest -Method POST -Body $accountLogin -ContentType "application/json" https://localhost:7187/v1/accounts/login
```

### Application

```powershell

# Create an Ask
$askCreateCommand = @{"body"="myAskBodyPs"; "title"="myAskTitlePs";"is_nsfw"=$true;"is_nsfl"=$false;IsNsfw=$true; images=(get-item -path ./aqua.png)}
Invoke-WebRequest -Method POST -Form $askCreateCommand -ContentType "multipart/form-data" https://localhost:7187/asks

# Add a Delivery to an Ask
$deliveryCreateCommand = @{"body"="myDeliveryBodyPs"; "title"="myDeliveryTitlePs";"is_nsfw"=$true;"is_nsfl"=$false;IsNsfw=$true; images=(get-item -path ./aqua.png)}
Invoke-WebRequest -Method POST -Form $deliveryCreateCommand -ContentType "multipart/form-data" https://localhost:7187/v1/asks/1/deliveries

# Add a Comment to an Ask
Invoke-WebRequest -Method POST -Body (@{"body"="myAskComment";} | ConvertTo-Json) -ContentType "application/json" https://localhost:7187/v1/asks/1/comments

# Add a Comment to a Delivery
Invoke-WebRequest -Method POST -Body (@{"body"="myDeliveryComment";} | ConvertTo-Json) -ContentType "application/json" https://localhost:7187/v1/asks/1/deliveries/1/comments

# Add a Tag to an Ask and all its child objects (i.e. Deliveries, Comments)
Invoke-WebRequest -Method POST -Body (@{"tag"=@{"value"="konosuba"}} | ConvertTo-Json)  -ContentType "application/json" https://localhost:7187/v1/asks/1/tags

# Get all Asks
curl http://localhost:80/api/v1/asks

# Get all Asks but include their Deliveries, Delivery Comments, and Ask Comments. (This done with OData)
curl "http://localhost:80/api/v1/asks?expand=deliveries(expand=comments),comments"

# Get all the Deliveries from the first Ask
curl http://localhost:80/api/v1/asks/1/deliveries
```