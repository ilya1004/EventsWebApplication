# Базовый образ для ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7002
EXPOSE 7012

# Установка curl в финальном образе
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Базовый образ для SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем .csproj файлы проектов
COPY EventsWebApplication.API/EventsWebApplication.API.csproj EventsWebApplication.API/
COPY EventsWebApplication.Application/EventsWebApplication.Application.csproj EventsWebApplication.Application/
COPY EventsWebApplication.Domain/EventsWebApplication.Domain.csproj EventsWebApplication.Domain/
COPY EventsWebApplication.Infrastructure/EventsWebApplication.Infrastructure.csproj EventsWebApplication.Infrastructure/

# Восстанавливаем зависимости
RUN dotnet restore EventsWebApplication.API/EventsWebApplication.API.csproj

# Копируем весь исходный код
COPY . .

# Сборка проекта
WORKDIR /src/EventsWebApplication.API
RUN dotnet build EventsWebApplication.API.csproj -c Release -o /app

# Публикация проекта
FROM build AS publish
RUN dotnet publish EventsWebApplication.API.csproj -c Release -o /app

# Финальный образ для выполнения
FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Копируем папку SeedImages в рабочую директорию контейнера
COPY SeedImages /app/images

ENTRYPOINT ["dotnet", "EventsWebApplication.API.dll"]
