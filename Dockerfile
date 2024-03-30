FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY PointOfSaleWeb.App/PointOfSaleWeb.App.csproj PointOfSaleWeb.App/
COPY PointOfSaleWeb.Models/PointOfSaleWeb.Models.csproj PointOfSaleWeb.Models/
COPY PointOfSaleWeb.Repository/PointOfSaleWeb.Repository.csproj PointOfSaleWeb.Repository/

RUN dotnet restore PointOfSaleWeb.App/PointOfSaleWeb.App.csproj

COPY . .

WORKDIR /src/PointOfSaleWeb.App

RUN dotnet build PointOfSaleWeb.App.csproj -c Release -o /app/build

FROM build as publish
RUN dotnet publish PointOfSaleWeb.App.csproj -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PointOfSaleWeb.App.dll"]
