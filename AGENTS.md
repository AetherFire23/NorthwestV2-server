# AGENTS.md

## Project Overview
.NET 10.0 ASP.NET game backend (React frontend). Digital board game with MediatR, EF Core, PostgreSQL.

## Build & Run
```powershell
dotnet build
dotnet run --project src/Api
```
- API: `http://localhost:5272`
- Swagger: `/swagger`
- Default args (in launchSettings): `--seed SeededCompany --scenario SeededCompany`

## Database
- PostgreSQL: `northwest` / `postgres` / `123` @ localhost:5432
- Docker: `docker run -d --name northwest-postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=123 -e POSTGRES_DB=northwest -p 5432:5432 postgres`

## Migrations
```powershell
dotnet ef migrations add <Name> --project src/Core
```

## Testing
- Unit: `dotnet test tests/Unit`
- Integration: `dotnet test tests/Integration` (uses Testcontainers PostgreSQL)

## Important Quirks
- **Dev mode auto-resets DB**: In Development, app deletes and remigrates DB on every startup (`Program.AppPhase`)
- CORS allows `localhost:5173` and `192.168.2.100:5173` only
- Session-based auth with cookies
- Connection string key: `ConnectionStrings:DefaultConnection`

## Solution Structure
| Directory | Purpose |
|-----------|---------|
| `src/Api` | Web API entrypoint (`Program` BuildPhase/AppPhase), `Composition/Composer` (`AddCoreServices`/`InitializeServices`), scenario launcher |
| `src/Core` | Use cases (MediatR handlers), domain, EF Core DbContext, migrations, seeding |
| `tests/Unit` | Unit tests (xUnit) |
| `tests/Integration` | Integration tests; `Tooling/IntegrationTestBase` + `NorthwestTestBase` (shared Postgres container, one DB per test) |
