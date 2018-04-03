# Stage 1 - build project
FROM microsoft/aspnetcore-build:2.0.6-2.1.101 AS builder
WORKDIR /source
COPY ./app .
WORKDIR /source/GStore.API
RUN dotnet restore
RUN dotnet build -c Release -o /build
RUN dotnet publish -c Release -o /publish

# Stage 2 - create container 
FROM microsoft/aspnetcore:2.0.6
WORKDIR /app
COPY --from=builder /publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Staging
ENTRYPOINT ["dotnet", "GStore.API.dll"]