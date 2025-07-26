FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build              # Base image with .NET 8.0 SDK for build
WORKDIR /app                                                # Set working directory

COPY *.sln .                                                # Copy solution file to the container
COPY FCG.FiapCloudGames/*.csproj ./FCG.FiapCloudGames/      # Copy the main project .csproj file
RUN dotnet restore                                          # Restore NuGet dependencies

COPY FCG.FiapCloudGames/. ./FCG.FiapCloudGames/             # Copy all source code to the container
WORKDIR /app/FCG.FiapCloudGames                             # Navigate to the project folder
RUN dotnet publish -c Release -o out                        # Publish the application in Release mode to the 'out' folder

FROM mcr.microsoft.com/dotnet/aspnet:8.0                    # Lightweight runtime image with ASP.NET Core
WORKDIR /app                                                # Set working directory for runtime
COPY --from=build /app/FCG.FiapCloudGames/out ./            # Copy published files from the build stage

ENTRYPOINT ["dotnet", "FCG.FiapCloudGames.dll"]             # Command to start the application