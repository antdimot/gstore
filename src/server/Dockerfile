# Build application
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app/src
COPY ./ /app/src/
RUN dotnet restore 
WORKDIR /app/src/GStore.API
RUN dotnet publish -c Release -o /app/publish

# Create image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY db/ /app/data/
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Staging
ENTRYPOINT ["dotnet", "GStore.API.dll"]
