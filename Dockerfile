# Stage 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

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
