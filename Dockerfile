# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

RUN apt-get update && apt-get install -y tzdata \
    && ln -fs /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime \
    && dpkg-reconfigure --frontend noninteractive tzdata

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Management.IAConversations.Api/Management.IAConversations.Api.csproj", "src/Management.IAConversations.Api/"]
COPY ["src/Management.IAConversations.Application/Management.IAConversations.Application.csproj", "src/Management.IAConversations.Application/"]
COPY ["src/Management.IAConversations.Domain/Management.IAConversations.Domain.csproj", "src/Management.IAConversations.Domain/"]
COPY ["src/Management.IAConversations.CrossCutting/Management.IAConversations.CrossCutting.csproj", "src/Management.IAConversations.CrossCutting/"]
COPY ["src/Management.IAConversations.Infrastructure/Management.IAConversations.Infrastructure.csproj", "src/Management.IAConversations.Infrastructure/"]

RUN dotnet restore "src/Management.IAConversations.Api/Management.IAConversations.Api.csproj"

COPY . .

RUN dotnet build "src/Management.IAConversations.Api/Management.IAConversations.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Management.IAConversations.Api/Management.IAConversations.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Management.IAConversations.Api.dll"]
