# Use the .NET SDK image as the base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the solution file and project folders to the container
COPY PointOfSaleWeb.App/PointOfSaleWeb.App.sln .
COPY PointOfSaleWeb.Models/ ./PointOfSaleWeb.Models/
COPY PointOfSaleWeb.Repository/ ./PointOfSaleWeb.Repository/

# Restore NuGet packages
RUN dotnet restore

# Build the app
RUN dotnet publish PointOfSaleWeb.App.sln -c Release -o out
# Set the entry point for the container
ENTRYPOINT ["dotnet", "out/PointOfSaleWeb.App.dll"]
