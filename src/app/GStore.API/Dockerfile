FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Staging

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY GStore.API/GStore.API.csproj GStore.API/
RUN dotnet restore GStore.API/GStore.API.csproj
COPY . .
WORKDIR /src/GStore.API
RUN dotnet build GStore.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GStore.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GStore.API.dll"]