FROM node:20 AS frontend-build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app/backend
COPY backend/*.csproj ./
RUN dotnet restore
COPY backend/ ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=backend-build /app/publish ./
COPY --from=frontend-build /app/dist ./wwwroot

EXPOSE 10000

ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT

ENTRYPOINT ["dotnet", "WordPlay.dll"]