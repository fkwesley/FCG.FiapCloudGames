# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY *.csproj ./

# Restore NuGet packages
RUN dotnet restore

# Copy all remaining source code
COPY . ./

# Build and publish the application to the /out directory
RUN dotnet publish -c Release -o /out

# Use the ASP.NET 8 runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published output from the build stage
COPY --from=build /out .

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "FCG.FiapCloudGames.dll"]