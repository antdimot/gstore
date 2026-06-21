# GStore — Agent Instructions

GStore is a geolocation content storage REST API built with ASP.NET Core 10.0 and MongoDB. It lets clients store arbitrary content tagged with GPS coordinates and retrieve it by proximity.

## Build & Test

```bash
# Build the API project
dotnet build src/GStore.API/GStore.API.csproj

# Run all tests
dotnet test src/GStore.Test/GStore.Test.csproj

# Start the full stack (API + MongoDB) via Docker
docker-compose up -d
```

API is available at `http://localhost:5010`. Swagger UI at `http://localhost:5010/swagger`. Health check at `http://localhost:5010/api/v1/status/check`.

## Project Structure

Three projects in `src/`:

| Project | Role |
|---------|------|
| `GStore.API` | ASP.NET Core 7 web layer — controllers, models (DTOs), security, configuration |
| `GStore.Core` | Business/data layer — domain entities, repository pattern, UnitOfWork, hashing |
| `GStore.Test` | MSTest unit tests for repositories |

## Architecture

- **Repository pattern**: Generic `Repository<T>` base in `GStore.Core/Data/Repository.cs`; specialized `GeoRepository<T>` for geo queries.
- **Unit of Work**: `UnitOfWork` in `GStore.Core/Data/UnitOfWork.cs` lazily caches repository instances.
- **DTO separation**: Domain entities live in `GStore.Core/Domain/`; API response types live in `GStore.API/Models/`.
- **BaseController**: All controllers extend `BaseController`, which provides lazy-initialized `SecurityService` and `UnitOfWork`, plus `GetUserId()` to extract the caller's `ObjectId` from JWT claims.

## Configuration

Configuration lives in `src/GStore.API/appsettings.*.json` under the `GStore` key:

| Key | Purpose |
|-----|---------|
| `DBconn` | MongoDB connection string (default: `mongodb://localhost:27017`) |
| `DBname` | Database name (`gstore`) |
| `password_salt` | Base64-encoded PBKDF2 salt |
| `token_appkey` | HMAC-SHA256 signing key for JWT |
| `token_expires_mins` | JWT lifetime in minutes (default: 40) |

Environment-specific files (`appsettings.Development.json`, `appsettings.Staging.json`) override the base file.

## Database Conventions

- **MongoDB** with the `MongoDB.Driver` package. The `DataContext` class holds the `IMongoDatabase` instance injected via DI.
- Collection name is derived from the entity type name in lowercase.
- BSON fields use short abbreviations to reduce document size (e.g., `un` = username, `pw` = password, `lo` = location).
- Geo queries use `$geoWithinCenterSphere` with radius in radians (Earth radius constant: 6378.1 km).
- Default query limit: **1000** items (defined in `Repository<T>`).
- All entities implement `IEntity<ObjectId>` and use `ObjectId` as the primary key (`_id`).

## Authentication & Security

- **JWT Bearer** authentication. Tokens are issued by `POST /api/v1/user/authenticate` (username + password form body).
- Passwords hashed with **PBKDF2-HMACSHA256** (100,000 iterations) via `HashUtility` in `GStore.Core`.
- The `"AdminApi"` authorization policy requires a `UserAuthz` claim containing `"admin"`.
- JWT audience: `GStoreAudience`, issuer: `GStoreIssuer`. Zero clock skew.
- CORS is currently permissive (`AllowAnyOrigin/Method/Header`) — marked as a temporary workaround in `Startup.cs`.

## API Conventions

- Route prefix: `/api/v{version:apiVersion}/` — current version is `v1`.
- All controller actions are `async Task<IActionResult>`.
- Serilog handles structured logging; error logs rotate daily to `../../log/error-{Date}.log`.
- `Utils.cs` in `GStore.API/Common/` contains MIME type helpers used by `GeoDataController`.

## Common Pitfalls

- **Salt/key configuration**: `password_salt` and `token_appkey` are required at runtime. Missing or mismatched values cause authentication failures silently.
- **MongoDB indexes**: The `GeoData` collection requires a `2dsphere` index on the location field for geo queries to work. Ensure it exists in MongoDB or call `InitCollection` in tests.
- **User soft-delete**: `User.Deleted` flag must be checked — the repository does not filter it automatically; the `UserController` filters it explicitly.
- **ObjectId parsing**: `GetUserId()` in `BaseController` parses `ObjectId` from claim string; pass a valid 24-char hex string or it throws.
