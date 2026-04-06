# AGENTS.md - NorthwestV2 Development Guide

This file provides guidance for AI agents working on the NorthwestV2 codebase.

## Project Overview

NorthwestV2 is a .NET 10 ASP.NET Core game application with a domain-driven design architecture. It uses PostgreSQL for data persistence and features a modular composition system for dependency injection.

### Solution Structure

```
NorthwestV2.sln
├── src/
│   ├── NortwestV2.Api/           # Web API entry point (port 5272)
│   ├── NorthwestV2.Application/ # Application layer
│   ├── NorthwestV2.Domain/      # Domain entities and logic
│   ├── NorthwestV2.Infrastructure/ # EF Core + PostgreSQL
│   ├── NorthwestV2.Seed/        # Database seeding services
│   └── NorthwestV2.Scenarios/   # Test scenario launcher
├── NorthwestV2.Features/        # Use cases, handlers, domain services
├── NorthwestV2.Compose/         # Composition root / DI installer
└── tests/
    ├── NorthwestV2.Unit/        # Unit tests (xUnit)
    └── NorthwestV2.Integration/ # Integration tests
```

---

## Build & Test Commands

### Build Solution
```powershell
dotnet build NorthwestV2.sln
```

### Build Specific Project
```powershell
dotnet build src/NortwestV2.Api/NortwestV2.Api.csproj
```

### Run Tests (All)
```powershell
dotnet test NorthwestV2.sln
```

### Run Unit Tests Only
```powershell
dotnet test tests/ERP.Unit/NorthwestV2.Unit.csproj
```

### Run Single Test (by method name)
```powershell
dotnet test tests/ERP.Unit/NorthwestV2.Unit.csproj --filter "FullyQualifiedName~PlayerFactoryTest.GivenPlayerFactory_WhenCreatePlayers_ThenPlayerCountIsCorrect"
```

### Run Single Test (by display name)
```powershell
dotnet test tests/ERP.Unit/NorthwestV2.Unit.csproj --filter "DisplayName=GivenPlayerFactory_WhenCreatePlayers_ThenPlayerCountIsCorrect"
```

### Run with Verbose Output
```powershell
dotnet test NorthwestV2.sln --verbosity normal
```

### Run API in Development
```powershell
dotnet run --project src/NortwestV2.Api/NortwestV2.Api.csproj
```

### Add Migration
```powershell
dotnet ef migrations add InitialCreate --project src/NorthwestV2.Infrastructure --startup-project src/NortwestV2.Api
```

---

## Code Style Guidelines

### General Conventions

- **.NET Version**: .NET 10.0
- **Language**: C# 13+ (nullable reference types enabled, implicit usings enabled)
- **Target Framework**: `net10.0`

### Project Configuration

Every `.csproj` should have:
```xml
<PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
</PropertyGroup>
```

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `PlayerFactory`, `LoginHandler` |
| Methods | PascalCase | `CreateFreshPlayersForGame` |
| Properties | PascalCase | `UserId`, `ActionPoints` |
| Private Fields | `_camelCase` | `_userRepository`, `_actionServices` |
| Interfaces | `I` Prefix | `IUserRepository`, `IPlayerRepository` |
| Constants | PascalCase | `INITIALIZATION_HEALTH = 100` |
| Enums | PascalCase | `Roles`, `AttackTypes` |

### File Organization

- **One class per file** (unless tightly coupled small classes)
- **Namespace matches folder path**: `NorthwestV2.Features.UseCases.Authentication.Login`
- **Test file location**: Mirror source structure in `tests/` folder
- **Test naming**: `<ClassName>Test` with methods like `Given<Condition>_When<Action>_Then<Expected>`

### Import Style

Use explicit namespaces rather than rely on implicit usings for domain types:
```csharp
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using Mediator;
```

### Entity Base Class

All entities inherit from `EntityBase` in `NorthwestV2.Features/Features/Shared/Entity/EntityBase.cs`:
```csharp
public class EntityBase
{
    [Key] public Guid Id { get; set; } = Guid.Empty;
}
```

### Dependency Injection

The project uses a custom composition system in `NorthwestV2.Compose/`. Install services via installers:
```csharp
public class DomainInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<PlayerFactory>();
    }
}
```

### Mediator Pattern (CQRS)

Use Mediator for all use cases:
- **Requests**: Implement `IRequest<TResponse>` or `IRequest`
- **Handlers**: Implement `IRequestHandler<TRequest, TResponse>`
- **Location**: `NorthwestV2.Features/UseCases/<Feature>/`

```csharp
public class LoginHandler : IRequestHandler<LoginRequest, LoginResult>
{
    public async ValueTask<LoginResult> Handle(LoginRequest request, CancellationToken ct)
    {
        // handler logic
    }
}
```

### Repository Pattern

Define repository interfaces in `NorthwestV2.Features/ApplicationsStuff/Repositories/`:
```csharp
public interface IUserRepository
{
    ValueTask<User> GetByUserName(string username);
}
```

Implement in `NorthwestV2.Infrastructure/Repositories/`.

### Error Handling

- Use custom exception classes for domain errors (e.g., `LoginException`)
- Use `throw new Exception("message")` for critical failures in handlers
- Validate inputs at the start of handlers

### Testing

- **Framework**: xUnit with `JetBrains.Annotations`
- **Test base**: No custom base class - use `[TestSubject(typeof(T))]` attribute
- **Assertions**: Use `Xunit.Assert` methods
- **Test output**: Use `ITestOutputHelper` with `TestOutputLogger<T>`

```csharp
[TestSubject(typeof(PlayerFactory))]
public class PlayerFactoryTest
{
    [Fact]
    public void GivenPlayerFactory_WhenCreatePlayers_ThenPlayerCountIsCorrect()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

### XML Documentation

Include XML docs for public APIs (enabled in `.csproj`):
```csharp
/// <summary>
/// Handles player login authentication.
/// </summary>
/// <remarks>
/// Currently uses plain-text password comparison. Should use hashing.
/// </remarks>
public class LoginHandler : IRequestHandler<LoginRequest, LoginResult>
```

---

## Database

- **Provider**: PostgreSQL (via `Npgsql.EntityFrameworkCore.PostgreSQL`)
- **ORM**: Entity Framework Core 10.0.2
- **Context**: `NorthwestV2.Infrastructure/NorthwestContext.cs`
- **Migrations**: Stored in `src/NorthwestV2.Infrastructure/Migrations/`

During development, the database is deleted and re-migrated on each startup in development mode.

---

## Frontend Integration

- **API URL**: `http://localhost:5272`
- **Frontend URL**: `http://localhost:5173` (Vite)
- **CORS**: Configured to allow credentials from frontend origins
- **Session**: Uses in-memory distributed cache

---

## Key Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.NET.Test.Sdk | 17.14.1 | Test runner |
| xUnit | 2.9.3 | Testing framework |
| Microsoft.EntityFrameworkCore | 10.0.2 | ORM |
| Npgsql.EntityFrameworkCore.PostgreSQL | 10.0.0 | PostgreSQL provider |
| Mediator | Latest | CQRS mediator |
