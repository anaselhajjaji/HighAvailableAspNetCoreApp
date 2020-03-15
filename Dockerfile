FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE $PORT

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
RUN dotnet build "SystemdHealthcheck.sln" -c Release -o /app/build

FROM build AS publish
# Should publish csproj, otherwise there is a runtime error about Newtonsoft.Json (don't undersand why...)
RUN dotnet publish "./SystemdHealthcheck/Healthcheck.Apis.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK CMD curl --fail http://localhost:$PORT/health || exit 1

ENTRYPOINT ["dotnet", "Healthcheck.Apis.dll"]
