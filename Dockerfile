FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build              # Imagem base com SDK .NET 8.0 para build
WORKDIR /app                                                # Define diretório de trabalho

COPY *.sln .                                                # Copia o arquivo da solução para o container
COPY FCG.FiapCloudGames/*.csproj ./FCG.FiapCloudGames/      # Copia o csproj do projeto principal
RUN dotnet restore                                          # Restaura as dependências NuGet

COPY FCG.FiapCloudGames/. ./FCG.FiapCloudGames/             # Copia todo o código fonte para o container
WORKDIR /app/FCG.FiapCloudGames                             # Navega para a pasta do projeto
RUN dotnet publish -c Release -o out                        # Publica a aplicação em Release na pasta out

FROM mcr.microsoft.com/dotnet/aspnet:8.0                    # Imagem runtime leve com ASP.NET Core
WORKDIR /app                                                # Define diretório de trabalho para runtime
COPY --from=build /app/FCG.FiapCloudGames/out ./            # Copia os arquivos publicados da build

ENTRYPOINT ["dotnet", "FCG.FiapCloudGames.dll"]             # Comando para iniciar a aplicação