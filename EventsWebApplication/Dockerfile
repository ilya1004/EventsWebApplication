FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7002
EXPOSE 7012

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY EventsWebApplication.API/EventsWebApplication.API.csproj EventsWebApplication.API/
COPY EventsWebApplication.Application/EventsWebApplication.Application.csproj EventsWebApplication.Application/
COPY EventsWebApplication.Domain/EventsWebApplication.Domain.csproj EventsWebApplication.Domain/
COPY EventsWebApplication.Infrastructure/EventsWebApplication.Infrastructure.csproj EventsWebApplication.Infrastructure/

RUN dotnet restore EventsWebApplication.API/EventsWebApplication.API.csproj

COPY . .

WORKDIR /src/EventsWebApplication.API
RUN dotnet build EventsWebApplication.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish EventsWebApplication.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY SeedImages /app/images
ENTRYPOINT ["dotnet", "EventsWebApplication.API.dll"]
