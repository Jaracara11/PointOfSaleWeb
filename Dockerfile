# Dockerfile

# Use a base image compatible with x86 architecture
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Copy the published application from the build stage
COPY --from=build /app /app

WORKDIR /app

# Set the entry point for the container
ENTRYPOINT ["dotnet", "PointOfSaleWeb.App.dll"]

# Optional: Set the command to specify additional options like URLs
CMD ["--urls", "http://+:80"]
