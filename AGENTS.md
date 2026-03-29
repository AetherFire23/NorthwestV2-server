# NorthwestV2 AGENTS.md

## output constraints

When building code, you need to always showme : 

0- analyze code context of the changed code (near-references) of the class you're workign on, (inheritors, usages, implementation)
The reason is I want you to check that the code you write doesn't have unintended consequences.
1- the changes you make (before - after)
2- Give me a run down of why you made those changes.
3- Run teh tests, and rollback all your changes if any of the tests are broken in consequence of your actions. 

## Project Overview
A .NET 8 board game with rich OOP domain models, CQRS via MediatR, and EF Core persistence.

## Tech Stack
- **.NET 8** / C#
- **MediatR** - CQRS pattern
- **EF Core** - PostgreSQL + Testcontainers
- **xUnit** - Integration and unit tests
- **DDD** - Clear layer separation

## Architecture Layers
```
src/
├── NorthwestV2.Domain/       # Entities, Domain Services, Value Objects, Actions logic
├── NorthwestV2.Application/ # Handlers, Repository interfaces, EF extensions
├── NorthwestV2.Infrastructure/ # Concrete implementations
├── NortwestV2.Api/           # API layer
└── NorthwestV2.Seed/        # Database seeding

tests/
├── ERP.Integration/          # Integration tests (Testcontainers)
└── ERP.Unit/                 # Unit tests
```

## Commands
```bash
dotnet build    # Build
dotnet test     # Run tests
dotnet watch test  # Watch mode
```

## Naming Conventions

### Classes
| Pattern | Example | Location |
|---------|---------|----------|
| `*Base` | `EntityBase`, `ActionBase` | Base classes |
| `*App` | `SpyglassProductionContributionActionApp` | Application layer |
| `*Action` | `SpyglassProductionContributionAction` | Domain layer |
| `*Requirement` | `RoomHasItemRequirement` | Domain constraints |
| `*Data` | `SpyglassFirstStageContributionData` | Records/data |

### Test Naming (GivenWhenThen)
```csharp
public async Task GivenPlayerWithScrapInInventory_WhenGetProductionAvailability_ThenCanExecuteAction()
```
- `Given*` - Precondition/setup
- `When*` - Action/event
- `Then*` - Assertion/result

### Test Files
- Test class: `{FeatureName}AppTest.cs`
- Attribute: `[TestSubject(typeof(ClassUnderTest))]`
- Base class: `NorthwestIntegrationTestBase`

## OOP Patterns

### Domain Entities
```csharp
// Abstract base with rich behavior
public abstract class ProductionItemBase : ItemBase
{
    public void Contribute(Player player) { ... }
    public abstract void OnProductionCompleted(Player player);
}

// Records for immutable state (EF Core compatible)
public record SpyglassFirstStageContributionData : StageContributionBase
{
    // Immutable updates via 'with' expression
    this.CurrentStageContribution = this.CurrentStageContribution 
        with { Contributions = this.CurrentStageContribution.Contributions + 1 };
}
```

### Entity Hierarchy
```
EntityBase (Id, Equals, GetHashCode)
├── ItemBase
│   ├── NormalItemBase
│   └── ProductionItemBase (LockedItems, CurrentStageContribution)
├── Player (Health, ActionPoints, Inventory, Room)
├── Room (RoomEnum, Inventory)
├── Game (Players, Rooms)
└── Inventory (Items)
```

## EF Core + OOP Strategy
- **JSON Polymorphism** for stage data: `[JsonPolymorphic]`, `[JsonDerivedType]`
- **Records** for immutable updates (EF tracks reference equality)
- **`with` expressions** for immutable mutations
- **Abstract base classes** for shared behavior

## Integration Test Patterns

### Setup Flow
```csharp
// 1. Create game with players
GameDataSeed seed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);

// 2. Teleport player to room
Guid playerId = await TeleportPlayerTo(seed, RequiredRoom);

// 3. Add required items
player.Inventory.Items.Add(new Scrap());

// 4. Execute action via MediatR
await Mediator.Send(new ExecuteActionRequest { ... });

// 5. RE-QUERY for assertions (CRITICAL!)
this._scope = RootServiceProvider.CreateScope();
Player playerAfter = Context.Players.First(x => x.Id == playerId);
```

### Critical Rule: Re-query After MediatR Calls
EF Core tracks entities loaded before `Mediator.Send()`. 
Always re-fetch entities for accurate assertions:
```csharp
// WRONG - stale data
var spyglass = player.Inventory.Find<UnfinishedSpyglass>(); 

// RIGHT - fresh from DB
this._scope = RootServiceProvider.CreateScope();
var room = Context.Rooms.Include(x => x.Inventory).ThenInclude(x => x.Items).First(...);
var spyglass = room.Inventory.Find<UnfinishedSpyglass>();
```

### Test Base Class
```csharp
public class MyFeatureTest : NorthwestIntegrationTestBase
{
    // Built-in:
    // - Mediator (from _scope)
    // - Context (from _scope)
    // - RootServiceProvider (for new scopes)
    
    // Pattern for fresh data:
    // this._scope = this.RootServiceProvider.CreateScope();
}
```

## Common Patterns

### Availability Check Pattern
```csharp
public InstantActionAvailability? DetermineAvailability(Player player)
{
    if (!player.Room.Inventory.Items.Any(x => x.ItemType == ItemTypes.UnfinishedSpyglass))
        return null;
    
    return new InstantActionAvailability { ... };
}
```

### Execute Pattern
```csharp
public override async Task Execute(ExecuteActionRequest request)
{
    Player player = await _playerRepository.GetPlayerWithRoomAndInventory(request.PlayerId);
    _domainAction.DoSomething(player);
    await _unitOfWork.SaveChangesAsync();
}
```

## Gotchas & Tips

1. **Scope management**: Create new scope (`RootServiceProvider.CreateScope()`) before assertions
2. **EF tracking**: Re-query entities after any `Mediator.Send()` call
3. **Business logic location**: Domain layer, not Application layer
4. **Stage advancement**: Uses `with` expression, EF Core handles reference properly
5. **Room vs Player inventory**: Be consistent - items belong to ONE inventory at a time

## Key Repository Methods
```csharp
IPlayerRepository
├── GetPlayerWithRoomAndInventory(playerId)
├── GetPlayerAndRoomAndInventoryAndGame(playerId)
└── ...
```

## Room Inventory Rule
Items for production/actions belong in **room inventories**, not player inventories.
Domain logic consistently uses `player.Room.Inventory.Find<T>()`.
