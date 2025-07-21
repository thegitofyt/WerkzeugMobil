# Use official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["WerkzeugMobil/WerkzeugMobil/WerkzeugMobil.csproj", "WerkzeugMobil/WerkzeugMobil/"]
RUN dotnet restore "WerkzeugMobil/WerkzeugMobil/WerkzeugMobil.csproj"

# Copy the rest of the code
COPY . .
WORKDIR "/src/WerkzeugMobil/WerkzeugMobil"
RUN dotnet publish -c Release -o /app/publish

# Use ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WerkzeugMobil.dll"]