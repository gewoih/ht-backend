FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HT.Api/HT.Api.csproj", "HT.Api/"]
COPY ["HT.Application/HT.Application.csproj", "HT.Application/"]
COPY ["HT.Domain/HT.Domain.csproj", "HT.Domain/"]
COPY ["HT.Infrastructure/HT.Infrastructure.csproj", "HT.Infrastructure/"]
RUN apt-get update && apt-get install -y curl && \
    curl -I https://api.nuget.org/v3/index.json

RUN dotnet restore "HT.Api/HT.Api.csproj"
COPY . .
WORKDIR "/src/HT.Api"
RUN dotnet build "HT.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HT.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HT.Api.dll"]
