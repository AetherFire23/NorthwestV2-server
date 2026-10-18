# CLAUDE.md

Guidance for Claude Code when working in this repository.

.NET 10 backend for a multiplayer digital board game (rooms, players, items, productions, combat). Mediator (source-generated), EF Core + PostgreSQL, session-cookie auth. The React frontend is the sibling repo `../NorthwestReactV2/` (it generates its API client with orval from this API's Swagger). The repo is standalone: no submodules, no shared-code repos. Layout and bootstrap deliberately mirror the sibling `../ActifNet` (CrtOrders) project; when in doubt about how something should be wired, look there.

## Commands (run from this folder)

```powershell
dotnet build NorthwestV2.sln
dotnet run --project src/Api/Api.csproj           # http://localhost:5272, Swagger at /swagger
dotnet test tests/Unit/Unit.csproj
dotnet test tests/Integration/Integration.csproj  # needs Docker running (Testcontainers)
dotnet test tests/Unit/Unit.csproj --filter "FullyQualifiedName~PlayerFactoryTest"   # one class / one test
```

Scripts use relative paths and must be run from `scripts/`:

```powershell
./"docker postgres.ps1"   # container northwest-postgres (postgres/123, db northwest, port 5432)
./migrations.ps1          # dotnet ef migrations add "initial <guid>" --project ../src/Core
./publishing.ps1          # dotnet publish -c Release -o ./publish
```

`dotnet-ef` is a global tool (`dotnet tool install --global dotnet-ef`); it is not pinned in a manifest.

The `http` launch profile passes `--seed SeededCompany --scenario SeededCompany`, so a normal F5 wipes and reseeds the database.

## Layout

| Project | Role |
|---|---|
| `src/Api` (`NorthwestV2.Api`, Web) | `Program.cs`, controllers, session helpers, Swagger schema filters, `Composition/Composer.cs`, `Scenarios/` (Playwright launcher) |
| `src/Core` (`NorthwestV2.Core`) | Domain (`Features/`), use cases (`UseCases/`), DI installers (`Installation/`, `Contexts/`), repositories, `NorthwestContext`, `Migrations/`, `Seeding/` |
| `tests/Unit` | Pure domain/handler tests with hand-built objects; references Core only |
| `tests/Integration` | Real DI graph on a real Postgres container |

**Namespaces do not match folders in Core.** Files kept their pre-merge namespaces: domain and use cases are `NorthwestV2.Features.*`, EF/repositories are `NorthwestV2.Infrastructure.*`, seeding is `NorthwestV2.Seed`, scenarios are `NorthwestV2.Scenarios`. Keep new files consistent with their neighbours rather than "fixing" a single file.

## Architecture

Request flow: `Controller` -> `Mediator.Send(Request)` -> `Handler` -> domain/entity logic -> `IUnitOfWork` save -> response DTO.

- **Bootstrap** (same shape as ActifNet): `Program.Main` = `BuildPhase(builder)`, `Build()`, `AppPhase(app, args)`, `Run()`. All service registration goes through `Composer.AddCoreServices(services, configuration)` in `src/Api/Composition/Composer.cs`, which calls the installers in order: `NorthwestContextInstaller`, `InfraInstaller`, `MediatorInstaller`, `DomainInstaller`, `ApplicationInstaller`. `Composer.InitializeServices` migrates the database. **Tests call the same two methods**, so anything registered in `Program.BuildPhase` only (controllers, session, seeding, CORS) is not available in integration tests; anything the domain needs must live in an installer.
- **DI is manual.** Repositories, actions, factories, `*App` classes and role initializers are registered by hand in the installers. A new class that is not added there fails at resolution time. Mediator handlers are discovered by the source generator, no registration needed.
- **Mediator must be Scoped** (`MediatorInstaller`). Singleton or transient works in tests but breaks in ASP.NET Core.
- **Config**: connection string key is `ConnectionStrings:DefaultConnection` (`NorthwestContextInstaller.ConnectionStringKey`). Missing key throws `MissingConnectionStringException`. The Development connection string is in `src/Api/appsettings.Development.json`.
- **Auth** is a server-side session plus an `nw_token` cookie (`AuthController`, `SessionExtensions.GetUserData()`); there is no `UseAuthentication`/`[Authorize]`. Controllers read `UserData` from the session and put ids into the request. `JwtTokenService` uses a hard-coded `JwtConfig`.
- **Randomness** goes through `IRandomProvider` (`RealRandom` in production, `FakeRandom` in `tests/Unit`). Never call `Random` directly in domain code.
- Enums serialize as strings so the frontend generates union types. Swagger needs the XML docs of both Api and Core (`GenerateDocumentationFile` is on in both csproj files).

## Database and migrations

- **`.gitignore` line 577 (`**/migrations/`) ignores `src/Core/Migrations/`.** Migrations are local-only and a fresh clone has none, so `MigrateAsync` has nothing to apply. Do not assume migrations exist, and do not delete the local ones without asking. Un-ignoring and committing one initial migration is the pending fix.
- `Program.AppPhase` in Development calls `EnsureDeletedAsync()` on every start, then `InitializeServices` migrates. Never point a Development run at a database you care about.
- `MigrationsFactory` is the design-time factory for `dotnet ef` and uses a fake connection string.
- After changing entities: `./migrations.ps1` from `scripts/`.

## Combat

Rules live in `docs/combat.md` (with **[assumed]** numbers in `CombatConstants`); `docs/combat-spec-vs-code.md` tracks what is implemented. `CombatAction.MakeTwoPlayerFightTogether` resolves a fight and writes Health and `PlayerLifeStatus` onto the players; `CombatAftermath` decides who gets which private game log; `CombatActionApp` saves. The combat action has three prompts: target, stance, attack type.

## Seeding and scenarios

- Seeders implement `ISeeder` (`src/Core/Seeding`) and are found by scanning the assembly of `SeededCompany`. Run with `--seed <TypeName> --scenario <name>`; both flags must be present or nothing is seeded.
- Scenarios (`src/Api/Scenarios`, `[Scenario("name")]` + `ScenarioBase`) drive Playwright against the frontend at `localhost:5173`. The launch call in `Program.cs` is commented out, so they are currently dormant.

## Tests

- **Unit** (`tests/Unit`): no database. Baseline today: the 10 `RoomFactoryTest` adjacency cases fail on `main` and did before the restructure. Do not treat them as regressions, and do not "fix" them as a side effect of unrelated work.
- **Integration** (`tests/Integration`): derive from `NorthwestTestBase : IntegrationTestBase`. One shared `postgres:18` container (`Tooling/SharedContainerFixture`, reused across runs), a fresh `nw_<guid>` database per test (created in `InitializeAsync`, dropped in `DisposeAsync`), real DI via `AddCoreServices`, migrations applied by `InitializeServices`. Available members: `Mediator`, `Context`, `PlayerRepository`, `Scope`, `GetServiceFromScope<T>()`, `RefreshScope()`.
- `RefreshScope()`/new scope does **not** refresh services already resolved from the old scope; re-read entities after it to avoid stale tracked state.
- To swap a service (e.g. `IRandomProvider` -> `FakeRandom`), override `OverrideServices(IServiceCollection)` in the test class.
- Tests seed themselves through Mediator requests (`RegisterRequest`, `CreateGameRequest`), usually via `ShareSeeds.ArrangeUntilGameCreation(mediator, context)`, which returns a `GameDataSeed` (UserIds, GameId, PlayerIds). Reuse it. `PlayerHelper.TeleportPlayerTo` moves a player for room-dependent tests.
- Game creation is random under `RealRandom`; order players by role (as `ShareSeeds` does) instead of relying on creation order.
- Logging from tests goes to xUnit output via `XUnitLoggerProvider` (EF Core categories are filtered out).
- Test style follows ActifNet: xUnit `[Fact]`, `Assert.*`, hand-written fakes, no mocking library, no FluentAssertions. Prefer `GivenX_WhenY_ThenZ` names and a blank line between Given, When and Then (no `// Arrange` comments) for new tests; do not rename existing tests just for style.

## Code style

Match the surrounding code; observed conventions:

- File-scoped namespaces, `ImplicitUsings` and `Nullable` enabled, one type per file.
- Constructor injection into `private readonly _camelCase` fields. No primary constructors.
- Requests/handlers per use case under `UseCases/<Area>/<Name>/` (`XRequest`, `XHandler`); controllers stay thin and only translate session + HTTP into a request.
- Do not add libraries, or `Async` suffixes to existing method names, without being asked.
- Comments explain why, not what. Add XML `<summary>` docs to public API-facing types since they feed Swagger.

## Gotchas

- `UseSession` runs before `MapControllers` now; keep it that way or `HttpContext.Session` throws.
- CORS origins (`localhost:5173`, `192.168.2.100:5173`) and the Kestrel port 5272 are hard-coded in `Program`.
- `AGENTS.md` is a shorter overlapping summary; keep both in sync if you change commands or layout.
- Commit messages in this repo are informal English; use the attribution lines the harness specifies.
