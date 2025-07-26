FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build        
WORKDIR /app                                          

COPY *.sln .                                          
COPY FCG.FiapCloudGames/*.csproj ./FCG.FiapCloudGames/
RUN dotnet restore                                    

COPY FCG.FiapCloudGames/. ./FCG.FiapCloudGames/       
WORKDIR /app/FCG.FiapCloudGames                       
RUN dotnet publish -c Release -o out                  

FROM mcr.microsoft.com/dotnet/aspnet:8.0              
WORKDIR /app                                          
COPY --from=build /app/FCG.FiapCloudGames/out ./      

ENTRYPOINT ["dotnet", "FCG.FiapCloudGames.dll"]       