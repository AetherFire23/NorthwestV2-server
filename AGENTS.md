# AGENTS.md

## Project Overview
.NET 10.0 ASP.NET game backend (React frontend). Digital board game with MediatR, EF Core, PostgreSQL.

## Build & Run
```powershell
dotnet build
dotnet run --project src/NortwestV2.Api
```
- API: `http://localhost:5272`
- Swagger: `/swagger`
- Default args (in launchSettings): `--seed SeededCompany --scenario SeededCompany`

## Database
- PostgreSQL: `northwest` / `postgres` / `123` @ localhost:5432
- Docker: `docker run -d --name northwest-postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=123 -e POSTGRES_DB=northwest -p 5432:5432 postgres`

## Migrations
```powershell
dotnet ef migrations add <Name> --project src/NorthwestV2.Infrastructure
```

## Testing
- Unit: `dotnet test tests/ERP.Unit`
- Integration: `dotnet test tests/ERP.Integration` (uses Testcontainers PostgreSQL)

## Important Quirks
- **Dev mode auto-resets DB**: In Development, app deletes and remigrates DB on every startup (`Program.cs:114-117`)
- CORS allows `localhost:5173` and `192.168.2.100:5173` only
- Session-based auth with cookies
- Frontend URL constant: `http://localhost:5173` (Program.cs:17)

## Solution Structure
| Directory | Purpose |
|-----------|---------|
| `src/NortwestV2.Api` | Web API entrypoint |
| `NorthwestV2.Features` | Use cases (MediatR handlers) |
| `src/NorthwestV2.Infrastructure` | EF Core DbContext, migrations |
| `NorthwestV2.Compose` | DI composition |
| `src/NorthwestV2.Seed` | Database seeding |
| `src/NorthwestV2.Scenarios` | Scenario launcher |
| `tests/ERP.Unit` | Unit tests (xUnit) |
| `tests/ERP.Integration` | Integration tests |