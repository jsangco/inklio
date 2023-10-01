# Introduction
Details on how to manage the Inklio SQL server

## Overview

The Inklio SQL server is hosted in Azure.

## Modifying the database

Making changes to the database is done by committing changes made to the sqlproj. This will trigger the build pipeline that automatically deploys changes to the database.

Deployments can also be done manually and locally using the steps below.

> **WARNING:** The build pipeline is free and limited in functionality. If SQL changes must be deployed before application changes are published, the SQL changes should be published in seperate and prior commit.

### Permissions

The dev machines IP address must be [granted access](https://learn.microsoft.com/en-us/azure/azure-sql/database/network-access-controls-overview?view=azuresql#allow-azure-services). This is done in the Azure Portal by the site admin.

### Prerequisites

 1. [.Net 7+ SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
 2. `dotnet tool install -g microsoft.sqlpackage`

### Steps

1. Build the Inklio.sql.sqlproj
    `dotnet build .\Inklio.Sql.sqlproj /p:NetCoreBuild=true`
2. Publish the dacpac
   `sqlpackage /Action:Publish /SourceFile:".\bin\Debug\Inklio.Sql.dacpac" /TargetConnectionString:"<db Connection string here>"`

> NOTE: The SqlPackage step takes about 2 minutes to complete.

### Troubleshooting

If there are major changes to the database, the build may fail with an error that says: "*The schema update is terminating because data loss might occur.*"

In this case appending `/p:BlockOnPossibleDataLoss=false` to the `sqlpackage` command can force the deployment.

### Resetting the database locally

The following script can be used to reset the container for the local db:
`docker build --no-cache -t inklio-inkliodb:latest .`