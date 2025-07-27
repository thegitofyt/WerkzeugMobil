# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy only csproj files to restore dependencies
COPY WerkzeugApi/WerkzeugApi.csproj WerkzeugApi/
COPY WerkzeugShared/WerkzeugShared.csproj WerkzeugShared/

RUN dotnet restore WerkzeugApi/WerkzeugApi.csproj

# Copy full source code
COPY . .

# Publish the API project (also copies the .sqlite file from csproj)
RUN dotnet publish WerkzeugApi/WerkzeugApi.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy published output (includes .dll and .sqlite)
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "WerkzeugApi.dll"]