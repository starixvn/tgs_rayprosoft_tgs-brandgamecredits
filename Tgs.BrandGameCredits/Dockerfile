#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.BrandGameCredits/Tgs.BrandGameCredits.csproj", "Tgs.BrandGameCredits/"]
COPY ["Tgs.BrandGameCredits.Common/Tgs.BrandGameCredits.Common.csproj", "Tgs.BrandGameCredits.Common/"]
COPY ["Tgs.BrandGameCredits.Core/Tgs.BrandGameCredits.Core.csproj", "Tgs.BrandGameCredits.Core/"]
COPY ["Tgs.BrandGameCredits.Repositories/Tgs.BrandGameCredits.Repositories.csproj", "Tgs.BrandGameCredits.Repositories/"]
RUN dotnet restore "./Tgs.BrandGameCredits/./Tgs.BrandGameCredits.csproj"
COPY . .
WORKDIR "/src/Tgs.BrandGameCredits"
RUN dotnet build "./Tgs.BrandGameCredits.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.BrandGameCredits.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.BrandGameCredits.dll"]