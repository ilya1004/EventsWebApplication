FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7003
EXPOSE 7013

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY EventsAppIdentityServer.API/EventsAppIdentityServer.API.csproj EventsAppIdentityServer.API/
COPY EventsAppIdentityServer.Application/EventsAppIdentityServer.Application.csproj EventsAppIdentityServer.Application/
COPY EventsAppIdentityServer.Domain/EventsAppIdentityServer.Domain.csproj EventsAppIdentityServer.Domain/
COPY EventsAppIdentityServer.Infrastructure/EventsAppIdentityServer.Infrastructure.csproj EventsAppIdentityServer.Infrastructure/

RUN dotnet restore EventsAppIdentityServer.API/EventsAppIdentityServer.API.csproj

COPY . .

WORKDIR /src/EventsAppIdentityServer.API
RUN dotnet build EventsAppIdentityServer.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish EventsAppIdentityServer.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EventsAppIdentityServer.API.dll"]
