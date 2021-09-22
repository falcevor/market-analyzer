FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base

ARG NUGET_REGISTRY

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MarketAnalyzer.Crawler/MarketAnalyzer.Crawler.csproj", "MarketAnalyzer.Crawler/"]
RUN dotnet restore "MarketAnalyzer.Crawler/MarketAnalyzer.Crawler.csproj" --source ${NUGET_REGISTRY}
COPY . .
WORKDIR "/src/MarketAnalyzer.Crawler"
RUN dotnet build "MarketAnalyzer.Crawler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarketAnalyzer.Crawler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketAnalyzer.Crawler.dll"]
