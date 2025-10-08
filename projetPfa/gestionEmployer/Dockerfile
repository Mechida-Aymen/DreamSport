# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef

# Copy project files
COPY gestionEmployer/gestionEmployer.csproj ./gestionEmployer/
COPY Shared.Messaging/Shared.Messaging.csproj ./Shared.Messaging/

# Restore dependencies
RUN dotnet restore ./gestionEmployer/gestionEmployer.csproj

# Copy the rest of the source
COPY gestionEmployer/ ./gestionEmployer/
COPY Shared.Messaging/ ./Shared.Messaging/

# Build the project
WORKDIR /src/gestionEmployer
RUN dotnet build -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "gestionEmployer.dll"]
