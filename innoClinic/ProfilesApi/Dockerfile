# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ProfilesApi/Profiles.Api.csproj", "ProfilesApi/"]
COPY ["Profiles.Application/Profiles.Application.csproj", "Profiles.Application/"]
COPY ["Profiles.Domain/Profiles.Domain.csproj", "Profiles.Domain/"]
COPY ["innoClinic.Abstractions/Shared.Abstractions.csproj", "innoClinic.Abstractions/"]
COPY ["Profiles.DataAccess/Profiles.DataAccess.csproj", "Profiles.DataAccess/"]
COPY ["Shared.ServiceDiscovery/Shared.ServiceDiscovery.csproj", "Shared.ServiceDiscovery/"]
RUN dotnet restore "./ProfilesApi/Profiles.Api.csproj"
COPY . .
WORKDIR "/src/ProfilesApi"
RUN dotnet build "./Profiles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Profiles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profiles.Api.dll"]