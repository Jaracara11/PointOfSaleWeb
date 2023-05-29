# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY PointOfSaleWeb.App/PointOfSaleWeb.App.csproj PointOfSaleWeb.App/
COPY PointOfSaleWeb.Models/PointOfSaleWeb.Models.csproj PointOfSaleWeb.Models/
COPY PointOfSaleWeb.Repository/PointOfSaleWeb.Repository.csproj PointOfSaleWeb.Repository/
COPY PointOfSaleWeb.App.sln .

RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "PointOfSaleWeb.App.dll", "--urls", "http://+:5000"]