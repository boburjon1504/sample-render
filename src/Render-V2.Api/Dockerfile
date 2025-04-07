FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# ❗️Fix the path here (no leading slash!)
COPY src/Render-V2.Api/Render-V2.Api.csproj src/Render-V2.Api/
RUN dotnet restore src/Render-V2.Api/Render-V2.Api.csproj

# Copy all source code
COPY . .

WORKDIR /src/Render-V2.Api
RUN dotnet build Render-V2.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Render-V2.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Render-V2.Api.dll"]
