# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY PointOfSaleWeb.App/PointOfSaleWeb.App.csproj PointOfSaleWeb.App/
COPY PointOfSaleWeb.Models/PointOfSaleWeb.Models.csproj PointOfSaleWeb.Models/
COPY PointOfSaleWeb.Repository/PointOfSaleWeb.Repository.csproj PointOfSaleWeb.Repository/

RUN dotnet restore PointOfSaleWeb.App/PointOfSaleWeb.App.csproj

# Copy the rest of the application and build
COPY . .
RUN dotnet publish PointOfSaleWeb.App/PointOfSaleWeb.App.csproj -c Release -o /app

# Stage 2: Create the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published artifacts from the build stage
COPY --from=build /app .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "PointOfSaleWeb.App.dll"]

# Optional: Set the command to specify additional options like URLs
CMD ["--urls", "http://+:80"]