FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/CringeGame.App"
RUN dotnet build "CringeGame.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CringeGame.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CringeGame.App.dll"]
