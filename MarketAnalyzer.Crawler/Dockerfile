FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG NUGET_REGISTRY
WORKDIR /src
COPY ["MarketAnalyzer.Crawler/MarketAnalyzer.Crawler.csproj", "MarketAnalyzer.Crawler/"]
COPY ["MarketAnalyzer.Data/MarketAnalyzer.Data.csproj", "MarketAnalyzer.Data/"]
RUN dotnet restore "MarketAnalyzer.Crawler/MarketAnalyzer.Crawler.csproj" ${NUGET_REGISTRY:+ --source} ${NUGET_REGISTRY:-}
COPY . .
WORKDIR "/src/MarketAnalyzer.Crawler"
RUN dotnet build "MarketAnalyzer.Crawler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarketAnalyzer.Crawler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketAnalyzer.Crawler.dll"]
