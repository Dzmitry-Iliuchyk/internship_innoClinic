# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["ProfilesApi/Profiles.Api.csproj", "ProfilesApi/"]
COPY ["Profiles.Application/Profiles.Application.csproj", "Profiles.Application/"]
COPY ["Profiles.DataAccess/Profiles.DataAccess.csproj", "Profiles.DataAccess/"]
COPY ["Profiles.Domain/Profiles.Domain.csproj", "Profiles.Domain/"]
COPY ["innoClinic.Abstractions/Shared.Abstractions.csproj", "innoClinic.Abstractions/"]
RUN dotnet restore "./innoClinic.Abstractions/Shared.Abstractions.csproj"
RUN dotnet restore "./Profiles.Domain/Profiles.Domain.csproj"
RUN dotnet restore "./Profiles.Application/Profiles.Application.csproj"
RUN dotnet restore "./Profiles.DataAccess/Profiles.DataAccess.csproj"
RUN dotnet restore "./ProfilesApi/Profiles.Api.csproj"
COPY . .
WORKDIR "/src"

RUN dotnet build "./innoClinic.Abstractions/Shared.Abstractions.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet build "./Profiles.Domain/Profiles.Domain.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet build "./Profiles.Application/Profiles.Application.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet build "./Profiles.DataAccess/Profiles.DataAccess.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet build "./ProfilesApi/Profiles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./ProfilesApi/Profiles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profiles.Api.dll"]