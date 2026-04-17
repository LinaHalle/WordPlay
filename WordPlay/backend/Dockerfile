FROM node:20 AS frontend-build
WORKDIR /app
COPY WordPlay/package*.json ./
RUN npm install
COPY WordPlay/ ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app

COPY WordPlay/backend/ backend/
RUN dotnet publish backend/backend.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=backend-build /app/publish ./
COPY --from=frontend-build /app/dist ./wwwroot

RUN ls -la

ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT

ENTRYPOINT ["dotnet", "backend.dll"]