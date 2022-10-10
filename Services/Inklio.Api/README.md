# Introduction
The Inklio API server.

## Building Docker

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