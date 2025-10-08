    # Base image
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
    WORKDIR /app
    EXPOSE 80
    EXPOSE 443

    # SDK image for build
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src

    # Copy project files
    COPY serviceMail/serviceMail.csproj ./serviceMail/
    COPY Shared.Messaging/Shared.Messaging.csproj ./Shared.Messaging/

    # Restore dependencies
    RUN dotnet restore ./serviceMail/serviceMail.csproj

    # Copy the rest of the source
    COPY serviceMail/ ./serviceMail/
    COPY Shared.Messaging/ ./Shared.Messaging/

    # Build the project
    WORKDIR /src/serviceMail
    RUN dotnet build -c Release -o /app/build

    # Publish the app
    FROM build AS publish
    RUN dotnet publish -c Release -o /app/publish

    # Final runtime image
    FROM base AS final
    WORKDIR /app
    COPY --from=publish /app/publish .

    ENTRYPOINT ["dotnet", "serviceMail.dll"]
