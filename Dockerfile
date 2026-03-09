FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/ ./src/
WORKDIR /src/src

RUN dotnet restore webapi.sln
RUN dotnet publish webapi.sln -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

EXPOSE 8080
ENV PORT=8080

COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "API.dll"]