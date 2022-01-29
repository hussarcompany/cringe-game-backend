FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CringeGame/CringeGame.csproj", "CringeGame/"]
RUN dotnet restore "CringeGame/CringeGame.csproj"
COPY . .
WORKDIR "/src/CringeGame"
RUN dotnet build "CringeGame.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CringeGame.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CringeGame.dll"]
