# Build application
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app/src
COPY ./GStore.Core/ /app/src/GStore.Core/
COPY ./GStore.API/ /app/src/GStore.API/
WORKDIR /app/src/GStore.API
RUN dotnet restore 
RUN dotnet publish -c Release -o /app/publish

# Create image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "GStore.API.dll"]
