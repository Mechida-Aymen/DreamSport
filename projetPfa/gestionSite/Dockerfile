# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install netcat-openbsd
RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*

# SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef

COPY gestionSite/gestionSite.csproj ./gestionSite/
COPY Shared.Messaging/Shared.Messaging.csproj ./Shared.Messaging/
RUN dotnet restore ./gestionSite/gestionSite.csproj

COPY gestionSite/ ./gestionSite/
COPY Shared.Messaging/ ./Shared.Messaging/
WORKDIR /src/gestionSite
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# ⬇️ This must point to the correct path of wait-for-it.sh
COPY gestionSite/wait-for-it.sh .
RUN chmod +x wait-for-it.sh

ENTRYPOINT ["./wait-for-it.sh", "sqlserver:1433", "--", "dotnet", "gestionSite.dll"]
