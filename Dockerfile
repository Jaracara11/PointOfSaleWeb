# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY PointOfSaleWeb.App/PointOfSaleWeb.App.csproj PointOfSaleWeb.App/
COPY PointOfSaleWeb.Models/PointOfSaleWeb.Models.csproj PointOfSaleWeb.Models/
COPY PointOfSaleWeb.Repository/PointOfSaleWeb.Repository.csproj PointOfSaleWeb.Repository/

RUN dotnet restore PointOfSaleWeb.App/PointOfSaleWeb.App.csproj

COPY . .
WORKDIR /src/PointOfSaleWeb.App

RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "PointOfSaleWeb.App.dll", "--urls", "http://+:5000"]