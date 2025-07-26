FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build              # Imagem base com SDK .NET 8.0 para build
WORKDIR /app                                                # Define diret�rio de trabalho

COPY *.sln .                                                # Copia o arquivo da solu��o para o container
COPY FCG.FiapCloudGames/*.csproj ./FCG.FiapCloudGames/      # Copia o csproj do projeto principal
RUN dotnet restore                                          # Restaura as depend�ncias NuGet

COPY FCG.FiapCloudGames/. ./FCG.FiapCloudGames/             # Copia todo o c�digo fonte para o container
WORKDIR /app/FCG.FiapCloudGames                             # Navega para a pasta do projeto
RUN dotnet publish -c Release -o out                        # Publica a aplica��o em Release na pasta out

FROM mcr.microsoft.com/dotnet/aspnet:8.0                    # Imagem runtime leve com ASP.NET Core
WORKDIR /app                                                # Define diret�rio de trabalho para runtime
COPY --from=build /app/FCG.FiapCloudGames/out ./            # Copia os arquivos publicados da build

ENTRYPOINT ["dotnet", "FCG.FiapCloudGames.dll"]             # Comando para iniciar a aplica��o