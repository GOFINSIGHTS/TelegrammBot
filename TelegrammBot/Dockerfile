# Stage 1: Base image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Stage 2: Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copying project files and restoring dependencies
COPY ["TelegrammBot/TelegrammBot.csproj", "TelegrammBot/"]
COPY ["Telegramm.Implementations/Telegramm.Implementations.csproj", "Telegramm.Implementations/"]
COPY ["Telegramm.Abstractions/Telegramm.Abstractions.csproj", "Telegramm.Abstractions/"]
COPY ["TelegrammBot.Services/TelegrammBot.Services.csproj", "TelegrammBot.Services/"]
COPY ["TelegrammBot.Domain/TelegrammBot.Domain.csproj", "TelegrammBot.Domain/"]
COPY ["TelegrammBot.Services.Abstractions/TelegrammBot.Services.Abstractions.csproj", "TelegrammBot.Services.Abstractions/"]
COPY ["TelegrammBot.Infrastructure.Implementation/TelegrammBot.Infrastructure.Implementation.csproj", "TelegrammBot.Infrastructure.Implementation/"]
COPY ["TelegrammBot.Infrastructure/TelegrammBot.Infrastructure.csproj", "TelegrammBot.Infrastructure/"]
COPY ["TelegrammBot.Infrastructure.PostgreSql/TelegrammBot.Infrastructure.PostgreSql.csproj", "TelegrammBot.Infrastructure.PostgreSql/"]

RUN dotnet restore "TelegrammBot/TelegrammBot.csproj"

# Copying all source files
COPY . .

# Building the project
WORKDIR "/src/TelegrammBot"
RUN dotnet build "TelegrammBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TelegrammBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Setting the environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Setting environment variables for secrets
ENV BotConfiguration__BotToken=${BotConfiguration__BotToken}
ENV BotConfiguration__MyChatID=${BotConfiguration__MyChatID}
ENV ConnectionStrings__DefaultConnection="Host=${PostgresHost};Port=5432;Database=${PostgresDatabase};Username=${PostgresUsername};Password=${PostgresPassword}"

# Creating a non-root user and setting permissions
RUN useradd -m appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "TelegrammBot.dll"]
