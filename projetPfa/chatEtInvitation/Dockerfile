# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080  
EXPOSE 8443  

# Install netcat-openbsd
RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef

# Copy project files
COPY chatEtInvitation/chatEtInvitation.csproj ./chatEtInvitation/
COPY Shared.Messaging/Shared.Messaging.csproj ./Shared.Messaging/

# Restore dependencies
RUN dotnet restore ./chatEtInvitation/chatEtInvitation.csproj

# Copy the rest of the source
COPY chatEtInvitation/ ./chatEtInvitation/
COPY Shared.Messaging/ ./Shared.Messaging/

# Build the project
WORKDIR /src/chatEtInvitation
RUN dotnet build -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


COPY chatEtInvitation/wait-for-it.sh .  
RUN chmod +x wait-for-it.sh

ENTRYPOINT ["./wait-for-it.sh", "sqlserver:1433", "--", "dotnet", "chatEtInvitation.dll"]
