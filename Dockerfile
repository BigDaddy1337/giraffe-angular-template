# Build frontend
FROM node AS frontend-build-env
WORKDIR /client
COPY src/client .
RUN npm install
RUN npm run build

# Build backend
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS backend-build-env
WORKDIR /backend
COPY src/ src/
COPY --from=frontend-build-env /client/dist src/client/dist
COPY tests/ tests/
RUN dotnet restore src/GiraffeAngularTemplate.fsproj
RUN dotnet restore tests/GiraffeAngularTemplate.Tests.fsproj
RUN dotnet test tests/GiraffeAngularTemplate.Tests.fsproj
RUN dotnet publish src/GiraffeAngularTemplate.fsproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=backend-build-env /backend/src/out .
ENTRYPOINT ["dotnet", "GiraffeAngularTemplate.dll"]