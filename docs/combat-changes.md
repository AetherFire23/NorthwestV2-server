# Combat rewrite: overview and key decisions

Implementation of the final `docs/combat.md` (2026-10-26). 74 files touched, about 3,300 lines added and 870 removed (tests included). Status per rule: `docs/combat-spec-vs-code.md`. Breaking API changes: `docs/changes/2026-10-26.md`.

Verification: `dotnet build NorthwestV2.sln` succeeds; unit tests 222/222 pass; integration tests 77/77 pass.

---

## 1. Before and after

| | Before | After |
|---|---|---|
| Toughness | 8 / 12 / 15, a "buffer" that resets when broken | 3 / 4 / 5: every full Toughness of damage costs 1 Health, leftover carries over |
| Strength | Toughness ± 1-point modifiers | Weapon basic damage × summed % modifiers (Health, Hope, stance, stun, weakness) |
| Strike order | Both sides strike simultaneously | Attacker first, defender strikes back if still up |
| Steps | Rounds only | Surprise attack → hit-and-run check → closing in → rounds → end of combat |
| Weapons | None | Knife, Sword, Pistol, Musket; inside/outside table; specialist bonuses; starting loadouts |
| Defender settings | One global `DefensiveCounter` (Default / Override) | `CombatReaction` per opponent × attack type: counter, weapon policy, faked Hope (acolytes) |
| Vigilance / surprise | Not implemented (`// TODO: Ask what vigilance means`) | `IsVigilant` setting, Sentry always vigilant, surprise strike when not vigilant |
| Stance logic | Fixed thresholds (any Strength deficit, "next blow kills me") | Look-ahead: the fighter plays out the rest of the fight on a copy and decides from the result |
| Information | Exact "x/100 health" log shown to the fighters and witnesses | Health is shown only in 20% increments; private logs follow the identification rules |

## 2. Architecture

```
CombatActionApp (application: load, save)
  │  loads attacker + defender with room, items and reactions (GetPlayerForCombat)
  ▼
CombatAction (domain entry point)
  ├─ DetermineAvailability / prompts / DetermineAttackerChoices
  ├─ CombatSetupFactory.Create  ── fixes weapons + modifiers  ──►  CombatSetup (2 × Combatant)
  ├─ CombatResolver.Resolve     ── pure, deterministic       ──►  FightResult
  └─ ApplyEndFightConditions    ── writes Health + LifeStatus onto Player
  ▼
CombatAftermath (IRandomProvider)  ──►  private CombatReports  ──►  GameLogs
```

| Layer | Types | Responsibility |
|---|---|---|
| Static data | `WeaponCatalog`, `WeaponProfile`, `RoomExposure`, `CombatConstants`, `CombatModifiers` | Every number in the spec, written out as authored data |
| Setup | `CombatSetupFactory`, `AttackerChoices`, `CombatSetup` | Everything the spec says is "fixed before the combat" |
| Resolution | `CombatResolver`, `CombatState`, `Combatant` | Order of a combat; no entity is touched |
| Outcome | `FightResult`, `FightExitType` | How it ended, statuses, weapons used, surprise flag |
| Information | `CombatAftermath`, `CombatReport` | Who learns what; the only place randomness enters |
| Settings | `CombatReaction` (entity), `SetCombatReaction*`, `SetVigilance*`, `GetMyCombatReactions*` | The defender's settings, made in advance |
| Exposure | `StatIncrements`, `HealthIncrement` / `HopeIncrement` DTO fields | 20% increments, never exact values |

Removed: `PlayerTempFightStats`, `DefensiveCounters`, `ChooseDefensiveCounter*`, `Player.CalculateStrength`, `Player.GetPlayerTempFightStats`, `Player.AttackTypes`, `Player.DefensiveCounter`.

---

## 3. Key architectural decisions

These are the choices that were not obvious from the spec or the existing code, why they were made, and what would break otherwise.

### 3.1 Every in-combat decision is a simulation, not a formula

The spec contains three conditional rules:

- *Closing in*: "the attacker closes in **if that gives him a better result** than ranged rounds".
- *Push hard*: break off at ≤20% Health "**and he would lose** the combat".
- *Hit-and-run*: stop "if the attacker's Health **would drop** to 40% or less **over the combat**".

The obvious approach is to write each one as a heuristic, such as comparing Strengths or estimating rounds to kill. The old code did exactly that, and it disagreed with the rules in edge cases: carried-over damage, alternating 5/6 Health losses, closing-in strikes, stance modifiers, and break-offs inside the alternative branch.

Instead, the resolver answers each "would" literally: it **clones the combat state and plays the rest of the fight with the same rules**, then reads the result.

- `WouldHitAndRunStop` plays `PlayFromClosingIn` on a clone.
- `IsClosingInBetter` plays two clones (close in vs stay at range) and compares them.
- `ShouldBreakOffUnderPushHard` plays the rounds on a clone with break-off disabled.

These nest. A hit-and-run look-ahead contains closing-in decisions, which can contain push hard look-aheads. The answers are exact because the same code is reused, so a heuristic can never drift away from the real resolution.

**What this required:**
- The resolver must be **pure**. It never writes to `Player`; only `CombatAction.ApplyEndFightConditions` does, after resolution. If it wrote to the entity, every simulation would corrupt the real players.
- `Combatant` and `CombatState` are **deep-cloneable**, including `WeaponsUsed`, so a branch played ahead does not leak weapons into the witness report.
- Push hard's own look-ahead runs with `canBreakOffUnderPushHard: false`. Otherwise "would I lose?" would ask itself the same question recursively and never answer.

**Termination.** The rounds loop always makes progress: an attacker with 0 Strength breaks off, and any positive Strength steadily fills the defender's damage pool. A `MAX_ROUNDS` guard turns a future bug into an exception instead of a hung request.

### 3.2 "Fixed at the start" is part of the data model, not a comment

The spec says modifiers are fixed before the combat and that damage taken during it does not change them. Health penalties apply only to melee, Hope penalties to both ranges, and stance only during the rounds.

So `Combatant` stores **two pre-summed percentages**, `MeleeModifierPercent` and `RangedModifierPercent`, computed once by `CombatSetupFactory`. The only modifier added at strike time is the stance, and only when the resolver is in the rounds. That makes it structurally impossible to apply a Health penalty to a pistol shot, or a stance bonus to a surprise attack or a closing-in strike.

### 3.3 Additive percentages and exact decimal damage

- **Additive.** The spec says "all modifiers apply directly to the basic damage" and "Strength can fall to 0". Multiplying the percentages never reaches 0 (0.8 × 0.7 × 0.7 × 0.8 ≠ 0), so the only consistent reading is a sum: −20 −30 −30 −20 = −100%. That is why a weakened, hopeless, wounded stun attacker can genuinely have 0 Strength and break off.
- **No rounding.** Strength is a `decimal` (Musket at −15% = 148.75), and only the conversion to Health uses `floor(pool / Toughness)`. Rounding each strike would silently lose up to 1 damage per strike. Over 20 rounds that changes who wins, and the result would depend on the order in which modifiers were rounded.

### 3.4 Life status is decided before the stun refund

At the end of the combat, 75% of the Health lost to stun damage is given back. If status were read from Health afterwards, a stunned player at 0 would get 75 back and look `Alive`. So:

- `Combatant.DownedBy` records, **at the moment Health first reaches 0**, which kind of damage did it.
- `Finish` computes `Dead` or `Stunned` from `DownedBy`, and only then applies the refund (rounded up: 99 lost → 75 back).
- This also covers mixed fights correctly. For example, a kill attack against a stun counter means the attacker can only be stunned and the defender can be killed. Within one combat, each fighter only ever receives one kind of damage.

### 3.5 Combat reactions are stored sparsely, and their defaults are computed

A defender has up to 44 settings (66 for acolytes). Storing all of them per player would mean seeding 12 × 11 × 2 rows per game and migrating them whenever the defaults change. Only reactions the player **changed** are stored. `Player.GetCombatReaction` falls back to `CombatReaction.CreateDefault`.

This matters beyond storage. One default is *dynamic*: an acolyte facing a stun fakes "the penalty matching their **tracked Hope**". Hope changes over the game, so the default cannot be precomputed into a row. It is stored as the intent `MatchTrackedHope` and resolved when the combat starts.

`CombatReaction.OpponentId` has deliberately **no navigation property**. A second foreign key to `Player` would give EF two relationships between the same pair of tables, which would need extra configuration and cascade-path decisions, for a navigation nothing reads.

### 3.6 Fitting a "choose a set of weapons" rule into single-choice prompts

The spec lets the attacker choose *which weapons* to use and then uses the strongest of them. The prompt system is a fixed, ordered list of screens with no conditional screens. Instead of extending the prompt system:

- There are **two single-choice prompts**: ranged (`None` + owned) and melee (`Unarmed` + owned). The spec picks the strongest weapon of each range anyway, so choosing one per range gives the same result as choosing a set.
- `None` on the ranged prompt is what makes an attack "melee", with no distance and no closing in. One choice encodes that rule.
- **A stun attack silently ignores the weapon prompts** instead of rejecting them. The frontend cannot hide screens based on an earlier answer, so validation would turn a legitimate "Stun + pistol selected" click into an error.

### 3.7 Allegiance must not leak through the API

Combat now touches the most sensitive secret in the game.

- The faked-Hope prompt is added **only for acolytes**, and only in that player's own action list. Loyals never see it, and nobody else sees anyone's prompts.
- `CombatReactionDto.FakedHopePenalty` is `null` for loyals instead of carrying an ignored default, so the response shape does not depend on hidden state in a way that could be compared later.
- When a weapon and bare hands deal the same damage, the defender **prefers bare hands**, so a concealed knife is not revealed in witness reports for no gain.
- `OtherPlayerDto` stays an explicit whitelist: only `HealthIncrement` was added, never exact Health or Hope.

Trade-off to be aware of: an acolyte's combat and reaction actions have one more prompt than a loyal's. That is only visible to the player himself, but a screen-share would show it.

### 3.8 Ambiguities resolved in the spec's own terms

| Ambiguity | Choice | Why |
|---|---|---|
| Attacker's closing-in comparison | Closing in vs **ranged rounds**, not vs "what the defender would then do" | Literal reading of the spec; a game-theoretic look-ahead of the opponent's reply would be a different rule |
| "Better result" between two non-wins, and break-offs | A break-off counts as a non-win; lower opponent Health is better; a tie stays at range | Keeps a total, deterministic order |
| Hit-and-run identification: 20% "after the surprise attack" vs 60% "no damage because not surprised" | The roll is keyed on **whether the defender was surprised**, not on damage dealt | A surprise strike can deal 0 Health (Strength 0), and the damage-based reading would then have no answer |
| Push hard "end of a round" | Checked after the defender's strike-back, never after the surprise or closing-in strikes | That is where a round ends; a single strike from above 20 to 0 still kills him, as the spec says |
| Witnesses "in the same room according to Meta" (presence for the whole tick) | Current room, conscious players only | There are no ticks yet; recorded as Open |

### 3.9 Data safety details

- New `ItemTypes` values were **appended at the end** of the enum. Inserting `Knife` next to `Sword` would have renumbered `Spyglass`, `Hammer`, `Ration` and the others, which EF stores as integers.
- `Sanity` → `Hope` was generated as a column **rename**, not a drop and re-add, so existing values survive.
- `Player.AttackTypes` was removed. The attack type is a per-attack choice; storing it on the entity was shared mutable state between unrelated attacks.

### 3.10 Tests check exact hand-computed values

Asserting "the attacker won" would pass with most bugs. Each `CombatResolverTest` scenario was worked out by hand, and the arithmetic is written in its comment. Examples:

- Equal unarmed Engineers, to the end: 22 Strength against Toughness 4 alternates 5 and 6 Health per strike. The defender goes down on strike 19, so the attacker ends at **10** Health.
- Ranger musket vs Engineer pistol, on deck: the attacker stays at range; the defender loses either way but leaves the Ranger at 55 instead of 75 by closing in, so he closes in.
- Marine musket vs Ranger musket: closing in would leave the Marine at 4 (his choice) or 72 (the Ranger's choice), so nobody closes in and he ends at **60**.

All of them passed on the first run. Since the closing-in and look-ahead logic was written independently of these numbers, that is reasonable evidence the implementation matches the spec.

Integration tests pick players **by role** (creation order is random) and add the faked-Hope screen only when the action lists it, because whether the Brute or Engineer is an acolyte is random.

---

## 4. What is still open

| Item | Blocked by |
|---|---|
| Vigilance cost (−1 Hope per tick, +2 rogue gauge for acolytes) | Tick system, rogue gauge |
| Causes of weakness (poison, laudanum, curse, exorcism) | Those actions; `Player.IsWeakened` is ready |
| Concealing an item | A conceal action; `ItemBase.IsConcealed` is ready and honoured |
| Witness presence for the whole tick | Tick system |
| How a stun ends; death at 0 Hope | Spec |
| Inuits (the Chaplain may not attack them either) | Inuits |
| Frontend | Regenerate the orval client: new prompts, removed `ChooseDefensiveStance`, new endpoint `GET game/me/combat-reactions`, new DTO fields |

Side note: the 10 `RoomFactoryTest` failures that CLAUDE.md lists as the known baseline no longer fail. They were not touched by this work.
