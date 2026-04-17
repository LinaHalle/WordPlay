# Stage 1: Build Frontend
FROM node:20 AS frontend-build
WORKDIR /app/frontend
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run ci

# Stage 2: Build Backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app/backend
COPY backend/*.csproj ./
RUN dotnet restore
COPY backend/ ./
RUN dotnet publish -c Release -o /app/publish

# Stage 3: Final Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=backend-build /app/publish .
# Kopiera de byggda frontend-filerna till wwwroot
COPY --from=frontend-build /app/dist ./wwwroot

EXPOSE 8080
ENTRYPOINT ["dotnet", "WordPlay.dll"] 

#dynamic port
CMD [ "sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet WordPlay.dll"]