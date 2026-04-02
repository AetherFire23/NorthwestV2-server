# AGENTS.md - NorthwestV2 Development Guide

This file provides guidance for AI agents working in this codebase.

## Project Overview

NorthwestV2 is a .NET 10.0 game backend (ERP system). The solution uses:
- **Framework**: .NET 10.0 with C#
- **Architecture**: Clean Architecture (Domain → Application → Infrastructure → API)
- **Pattern**: Mediator for CQRS, Entity Framework Core for data access
- **Testing**: xUnit with coverage collection

## Build & Test Commands

### Build
```powershell
dotnet build NorthwestV2.sln
```

### Run Tests (All)
```powershell
dotnet test NorthwestV2.sln
```

### Run Single Test (Unit Tests)
```powershell
# By test class name
dotnet test tests/ERP.Unit/NorthwestV2.Unit.csproj --filter "FullyQualifiedName~PlayerFactoryTest"

# By specific test method
dotnet test tests/ERP.Unit/NorthwestV2.Unit.csproj --filter "FullyQualifiedName~PlayerFactoryTest.GivenPlayerFactory_WhenCreatePlayers_ThenPlayerCountIsCorrect"
```

### Run Single Test (Integration Tests)
```powershell
dotnet test tests/ERP.Integration/NorthwestV2.Integration.csproj --filter "FullyQualifiedName~TestClassName.TestMethodName"
```

### Run All Tests in a Project
```powershell
dotnet test tests/ERP.Unit/NorthwestV2.Unit.csproj
```

## Solution Structure

```
src/
├── NorthwestV2.Domain/          # Core domain entities and logic
├── NorthwestV2.Application/      # Application services, Mediator handlers
├── NorthwestV2.Infrastructure/  # EF Core, external integrations
├── NortwestV2.Api/              # ASP.NET Core Web API
├── NorthwestV2.Seed/             # Database seeding
└── commons/                      # Shared libraries (AetherFire23.*)

tests/
├── ERP.Unit/                    # Unit tests
└── ERP.Integration/             # Integration tests
```

## Code Style Guidelines

### Namespaces
- **Convention**: `AetherFire23.ERP.Domain.*` (note: uses AetherFire23 prefix despite project name)
- Example: `AetherFire23.ERP.Domain.Entity`, `AetherFire23.ERP.Domain.Features.Actions.General.Combat`

### Classes & Types
- Use `class` for entities and services
- Use `record` for DTOs and value objects
- Use `interface` for abstractions (e.g., `IRandomProvider`)

### Properties
```csharp
// Required properties with init
public required User User { get; init; }

// Optional with default
public int ActionPoints { get; set; } = 8;

// Private setters when needed
public Guid Id { get; private set; }
```

### Naming
- **Classes/Methods**: PascalCase (`PlayerFactory`, `CreateFreshPlayersForGame`)
- **Properties**: PascalCase (`UserId`, `HashedPassword`)
- **Private fields**: (not commonly used - rely on local variables)
- **Interfaces**: Prefix with `I` (`IRandomProvider`)

### Nullable & Types
- Enabled via `<Nullable>enable</Nullable>` in csproj
- Use `string?` for nullable strings
- Use `Guid` for identifiers (not int)
- Collections: `List<T>`, `IEnumerable<T>`, `[T]` (collection expressions)

### Import Style
- Use global using (ImplicitUsings enabled)
- Explicit namespaces only when needed for disambiguation

### Documentation
- Use XML `<summary>` comments for public APIs
- Include parameter descriptions in doc comments

### Error Handling
- Use `throw new Exception("message")` for domain logic errors
- Validate inputs in constructors or factory methods

### Constants
```csharp
public const int INITIALIZATION_HEALTH = 100;
// Or
public static class GameSettings
{
    public const int RequiredPlayerCountToStartGame = 12;
}
```

## Testing Conventions

### Test Structure (xUnit)
```csharp
[TestSubject(typeof(PlayerFactory))]
public class PlayerFactoryTest
{
    private readonly ITestOutputHelper _outputHelper;

    public PlayerFactoryTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void TestName()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

### Test Helpers
- Use `FakeRandom` for deterministic random testing
- Use `TestOutputLogger<T>` for capturing logs in tests

### Project References in Tests
```xml
<ItemGroup>
  <ProjectReference Include="..\..\src\NorthwestV2.Application\NorthwestV2.Application.csproj" />
  <ProjectReference Include="..\..\src\NorthwestV2.Domain\NorthwestV2.Domain.csproj" />
</ItemGroup>
```

## Common Patterns

### Entity Base
```csharp
public class EntityBase
{
    [Key] public Guid Id { get; set; } = Guid.Empty;
    // Equals/HashCode based on Id
}
```

### Domain Actions (Availability)
```csharp
public class CombatAction
{
    public ActionWithTargetsAvailability DetermineAvailability(Player caster, List<Player> otherPlayersInSameRoom)
    {
        // Return availability with requirements and prompts
    }

    public FightResult MakeTwoPlayerFightTogether(Player attackerPlayer, Player defenderPlayer)
    {
        // Execute the action
    }
}
```

### Role Initializers
- Located in `src/NorthwestV2.Domain/GameStart/RoleInitializations/PlayerInitializers/`
- Implement `RoleInitializer` abstract class
- Auto-discovered via reflection in `DomainInstaller`

## Database

- Uses Entity Framework Core
- Migrations managed via scripts in `scripts/migrations.ps1`
- Uses PostgreSQL (see `scripts/docker postgres.ps1`)

## Key Files

- `NorthwestV2.sln` - Solution file
- `src/NorthwestV2.Domain/Entity/Player.cs` - Core player entity
- `src/NorthwestV2.Domain/GameStart/PlayerFactory.cs` - Factory for creating players
- `tests/ERP.Unit/PlayerFactoryTest.cs` - Example tests

## Notes

- The project uses a subdomain naming convention (`AetherFire23`) that differs from the solution name (`NorthwestV2`)
- Domain layer has no external dependencies (pure C#)
- Application layer depends on Domain + Mediator + EF Core