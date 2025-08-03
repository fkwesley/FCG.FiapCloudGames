# Stage 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Set the environment variables for New Relic
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER="{36032161-FFC0-4B61-B559-F6C5D41BAE5A}"
ENV CORECLR_PROFILER_PATH="/newrelic-netcore20-agent/libNewRelicProfiler.so"
ENV NEW_RELIC_DISTRIBUTED_TRACING_ENABLED=true

# this variables are replaced at runtime ci-cd pipeline
ENV NEW_RELIC_LICENSE_KEY=""
ENV NEW_RELIC_APP_NAME=""

# Install New Relic dependencies
RUN apt-get update && apt-get install -y wget tar \
  && wget https://download.newrelic.com/.net_agent/latest_release/NewRelic-dotnet-agent-linux-x64.tar.gz \
  && mkdir /newrelic-netcore20-agent \
  && tar -xzf NewRelic-dotnet-agent-linux-x64.tar.gz -C /newrelic-netcore20-agent

# Copy full solution and project folders
# This assumes you are building from the root of the repository
COPY . .

# Restore NuGet packages
RUN dotnet restore FCG.FiapCloudGames.sln

# Build the application in Release mode
RUN dotnet build FCG.FiapCloudGames.sln -c Release --no-restore

# Publish the application
RUN dotnet publish FCG.API/FCG.API.csproj -c Release -o /app/publish --no-restore

# Stage 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set working directory
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Set the entry point of the application
ENTRYPOINT ["dotnet", "FCG.API.dll"]
