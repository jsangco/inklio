#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY ["bin/Release/net7.0/", "."]
ENV ASPNETCORE_URLS http://+.:5000
ENTRYPOINT ["dotnet", "Inklio.Api.dll"]