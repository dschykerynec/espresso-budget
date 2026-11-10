FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json ./
COPY backend/src/EspressoBudget.Migrations/EspressoBudget.Migrations.csproj backend/src/EspressoBudget.Migrations/
RUN dotnet restore backend/src/EspressoBudget.Migrations/EspressoBudget.Migrations.csproj
COPY backend/src/EspressoBudget.Migrations/ backend/src/EspressoBudget.Migrations/
RUN dotnet publish backend/src/EspressoBudget.Migrations/EspressoBudget.Migrations.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
COPY --from=build /app/publish ./
USER ${APP_UID}
ENTRYPOINT ["dotnet", "EspressoBudget.Migrations.dll"]