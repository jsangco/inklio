# Introduction
Details on how to manage the Inklio SQL server

## Overview

 The Inklio SQL server is hosted in Azure.

 ## Modifying the database

 Changes to the database are done by publishing dacpac files. These files are built using the `Inklio.Sql.sqlproj` file. Below are the steps to build and deploy SQL changes. In the future these changes can be automated once Microsoft properly supports deployments on linux VMs

 ### Prerequisites

 1. [SqlPackage](https://docs.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage-download?view=sql-server-ver16). 
 2. [.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### Steps

1. Build the Inklio.sql.sqlproj
    `dotnet build .\Inklio.Sql.sqlproj /p:NetCoreBuild=true`
2. Publish the dacpac
   SqlPackage.exe /Action:Publish /SourceFile:".\bin\Debug\Inklio.Sql.dacpac" /TargetConnectionString:"<db Connection string here>"

> NOTE: The SqlPackage step takes about 2 minutes to complete.