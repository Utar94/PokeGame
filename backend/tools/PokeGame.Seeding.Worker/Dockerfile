#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tools/PokeGame.Seeding.Worker/PokeGame.Seeding.Worker.csproj", "tools/PokeGame.Seeding.Worker/"]
RUN dotnet restore "./tools/PokeGame.Seeding.Worker/PokeGame.Seeding.Worker.csproj"
COPY . .
WORKDIR "/src/tools/PokeGame.Seeding.Worker"
RUN dotnet build "./PokeGame.Seeding.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PokeGame.Seeding.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokeGame.Seeding.Worker.dll"]