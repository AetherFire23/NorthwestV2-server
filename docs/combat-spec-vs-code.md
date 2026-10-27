# Combat: spec vs code

Status of `docs/combat.md` (final version) in the code. **Done** is implemented and covered by tests, **Open** is not implemented, usually because the subsystem it depends on does not exist yet.

Code lives in `src/Core/Features/Actions/General/Combat`:

| Piece | Role |
|---|---|
| `StartCombat/Domain/WeaponCatalog` | Weapon table (inside/outside), unarmed = Toughness × 5, specialist bonuses, owned/strongest weapons |
| `StartCombat/Domain/CombatModifiers` | Health, Hope, faked Hope and stance percentages |
| `StartCombat/Domain/CombatSetupFactory` | Fixes weapons and modifiers of both fighters before the combat; reads the defender's `CombatReaction` |
| `StartCombat/Domain/CombatResolver` | Surprise attack, hit-and-run check, closing in, rounds, push hard, end of combat. Decisions are made by playing alternatives ahead on a cloned `CombatState` |
| `StartCombat/Domain/CombatAftermath` | Private game logs: death, identification (random rolls through `IRandomProvider`), attacker outcome, witness proof |
| `StartCombat/Domain/CombatAction` | Availability, prompts, writing Health and `PlayerLifeStatus` back onto the players |
| `SetCombatReaction/`, `SetVigilance/` | The defender's settings, made in advance |
| `Shared/Entity/CombatReaction` | One stored reaction per (owner, opponent, attack type); missing rows mean the defaults |

## Status

| Spec section | Status | Notes |
|---|---|---|
| 1. Deterministic outcome, same room, uses the defender's reactions | **Done** | The only randomness is identification after an early hit-and-run stop |
| 1. Chaplain can never attack humans | **Done** | Unfulfilled requirement on the combat action. Inuits do not exist yet |
| 2. Health 0-100, shown in 20% increments | **Done** | `StatIncrements`; `HealthIncrement` on `OtherPlayerDto` and `GetMyPlayerResponse`, never the exact value |
| 2. Hope (loyals), known in 20% increments | **Done** | `Player.Sanity` renamed `Hope`; `HopeIncrement` on `GetMyPlayerResponse` only. Death at 0 Hope is **Open** |
| 2. Hope (acolytes) has no mechanical effect | **Done** | Acolytes never suffer the Hope penalty; they apply a faked one |
| 2. Vigilance: persistent, secret, Sentry for free | **Done** | `Player.IsVigilant`, `SetVigilance` action, `IsEffectivelyVigilant` |
| 2. Vigilance costs 1 Hope per tick (+2 rogue gauge for acolytes) | **Open** | No tick system and no rogue gauge yet |
| 2. Toughness 3 / 4 / 5, 1 Health per Toughness of damage, leftover discarded | **Done** | `ToughnessInitializationConstants`, `Combatant.ReceiveStrike` |
| 2. Weapon table, specialists, strongest weapon | **Done** | Knife, Sword, Pistol, Musket items; starting loadouts per `docs/conception/items.md` |
| 2. Inside / outside | **Done** | `RoomExposure`: CrowsNest, MainDeck, ForeCastle, QuarterDeck are outside |
| 2. Health (melee) and Hope penalties, stance, stun, weakness; additive, can reach 0 | **Done** | Percentages are summed and applied once to the basic damage, clamped at 0; Strength is `decimal` |
| 2. Causes of weakness (poison, laudanum, curse, exorcism) | **Open** | `Player.IsWeakened` exists, nothing sets it |
| 2. Explosives are not weapons | **Done** | Not in `WeaponCatalog` |
| 3. Kill / stun damage, stun always unarmed | **Done** | A stun attack ignores the weapon prompts |
| 4. Attacker choices: attack type, weapons, faked Hope (acolytes), stance | **Done** | Prompts: target, stance, attack type, ranged weapon (`None` + owned), melee weapon (`Unarmed` + owned), faked Hope (acolytes only) |
| 4. Push hard, to the end, hit-and-run | **Done** | Push hard and hit-and-run look ahead by playing the rest of the combat |
| 5. Combat reactions per opponent and attack type, weapon policy, faked Hope | **Done** | `SetCombatReaction` action, `GET game/me/combat-reactions` |
| 5. Concealed weapons | **Partly** | `ItemBase.IsConcealed` is honoured by the "not concealed" policy; no action conceals an item yet |
| 5. Default settings | **Done** | `CombatReaction.CreateDefault` |
| 6. Surprise attack, doubled in melee | **Done** | |
| 6. Closing in, "better result", free strike | **Done** | Attacker compares closing in with ranged rounds, then the defender does; ties stay at range |
| 6. Rounds, 0-Strength break-off, no free strike on break-off | **Done** | |
| 7. Leftover discarded, 75% of stun Health loss given back, rounded up | **Done** | Status is decided before the refund: a stunned fighter stays `Stunned` |
| 8. Hit-and-run identification 20% / 60%, defender knows he was targeted | **Done** | The roll depends on whether the defender was surprised |
| 8. Defender identifies the attacker after a combat, unless killed | **Done** | |
| 8. Dead get only the fact of death; witnesses get proof and weapons | **Done** | Witnesses are the conscious players in the current room |
| 8. Witnesses "in the same room according to Meta" (presence for the whole tick) | **Open** | No ticks: presence is the current room |

## Not in the spec, decided during implementation

- Dead and stunned players can neither attack nor be targeted. How a stun ends is **Open**.
- The attacker gets a private log about how it ended (defender dead, knocked out, or the attacker broke off).
- A defender picks bare hands over a weapon of equal damage, so he does not reveal a weapon for nothing.
- `Player.AttackTypes` and `Player.DefensiveCounter` were removed: the attack type is a per-attack choice and reactions are per opponent.

## Tests

`tests/Unit/Features/Actions/General/Combat`: `WeaponCatalogTest`, `CombatantTest`, `CombatSetupFactoryTest`, `CombatResolverTest` (each scenario worked out by hand in its comment), `CombatActionTest`, `CombatAftermathTest`, `SetCombatReactionActionTest`, `CombatStatsTest` (toughness per role, starting weapons). `tests/Unit/Features/StatIncrementsTest`.

`tests/Integration/Features/Actions/General/Combat`: `CombatActionAppTest`, `SetCombatReactionActionAppTest` (reaction matrix, stored reaction used in a fight, vigilance).
