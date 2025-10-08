# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ApiGateway/ApiGateway.csproj ./ApiGateway/
COPY Shared.Messaging/Shared.Messaging.csproj ./Shared.Messaging/

# Restore dependencies
RUN dotnet restore ./ApiGateway/ApiGateway.csproj

# Copy the rest of the source
COPY ApiGateway/ ./ApiGateway/
COPY Shared.Messaging/ ./Shared.Messaging/

# Build the project
WORKDIR /src/ApiGateway
RUN dotnet build -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
