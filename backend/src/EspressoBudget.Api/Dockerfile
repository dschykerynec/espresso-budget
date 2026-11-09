FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json ./
COPY backend/src/EspressoBudget.Api/EspressoBudget.Api.csproj backend/src/EspressoBudget.Api/
RUN dotnet restore backend/src/EspressoBudget.Api/EspressoBudget.Api.csproj
COPY backend/src/EspressoBudget.Api/ backend/src/EspressoBudget.Api/
RUN dotnet publish backend/src/EspressoBudget.Api/EspressoBudget.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish ./
USER ${APP_UID}
EXPOSE 8080
ENTRYPOINT ["dotnet", "EspressoBudget.Api.dll"]