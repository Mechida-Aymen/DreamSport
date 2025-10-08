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
COPY gestionReservation/gestionReservation.csproj ./gestionReservation/
COPY Shared.Messaging/Shared.Messaging.csproj ./Shared.Messaging/

# Restore dependencies
RUN dotnet restore ./gestionReservation/gestionReservation.csproj

# Copy the rest of the source
COPY gestionReservation/ ./gestionReservation/
COPY Shared.Messaging/ ./Shared.Messaging/

# Build the project
WORKDIR /src/gestionReservation
RUN dotnet build -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "gestionReservation.dll"]
