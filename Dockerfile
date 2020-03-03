FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE $PORT

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["SystemdHealthcheck.csproj", "./"]
RUN dotnet restore "./SystemdHealthcheck.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SystemdHealthcheck.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SystemdHealthcheck.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK CMD curl --fail http://localhost:$PORT/health || exit 1

ENTRYPOINT ["dotnet", "SystemdHealthcheck.dll"]
