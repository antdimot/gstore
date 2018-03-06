# Stage 1 - build project
FROM microsoft/aspnetcore-build:2.0 AS builder
WORKDIR /source
COPY ./src/api .
WORKDIR /source/GStore.API
RUN dotnet restore
RUN dotnet build -c Release -o /build
RUN dotnet publish -c Release -o /publish

# Stage 2 - create container 
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=builder /publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Staging
ENTRYPOINT ["dotnet", "GStore.API.dll"]