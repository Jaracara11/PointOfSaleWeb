# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copy csproj files and restore dependencies
COPY PointOfSaleWeb.App/PointOfSaleWeb.App.csproj PointOfSaleWeb.App/
COPY PointOfSaleWeb.Models/PointOfSaleWeb.Models.csproj PointOfSaleWeb.Models/
COPY PointOfSaleWeb.Repository/PointOfSaleWeb.Repository.csproj PointOfSaleWeb.Repository/

RUN dotnet restore PointOfSaleWeb.App/PointOfSaleWeb.App.csproj

# Copy the rest of the application and build
COPY . .

RUN dotnet publish PointOfSaleWeb.App/PointOfSaleWeb.App.csproj -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

COPY --from=build /app /app

WORKDIR /app

ENTRYPOINT ["dotnet", "PointOfSaleWeb.App.dll"]
CMD ["--urls", "https://pointofsalewebapp.azurewebsites.net", "http://+:5000"]
