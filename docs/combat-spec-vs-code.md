# Combat: spec vs code

Audit of `docs/combat.md` (original text) against the code in `src/Core/Features/Actions/General/Combat`, `Player.cs` and the role initializers, followed by what was changed. Status after the change is marked **Fixed**, **Partly** or **Open**.

## What the code already got right

- Role toughness tiers match the spec exactly: Weak 8 (Chaplain, Scholar, Doctor), Tough 15 (Marine, Brute, Sapper), Normal 12 (the other six).
- Health starts at 100; fights are deterministic rounds with simultaneous Strength application; Toughness at 0 converts to Health loss and resets.
- Unarmed Strength equals Toughness.
- To the end gives the attacker only a small boost and never flees; hit-and-run exits before damage when weaker; push hard flees when about to die.
- Toughness carries across rounds within a fight.

## Divergences found

| # | Spec | Code before | Status |
|---|------|-------------|--------|
| 1 | Only Health damage persists after a fight | `CombatActionApp` discarded the `FightResult`; Health was never written back | **Fixed**: `ApplyEndFightConditions` writes Health and life status to both players |
| 2 | Down at Health 0 | Loop ended on `attacker <= 0 \|\| defender < 0`, winner chosen with `attacker >= 0`: a defender at exactly 0 survived, an attacker at 0 "won", simultaneous knockout went to the attacker | **Fixed**: down at `<= 0`, winner is whoever still has Health, mutual knockout has no winner |
| 3 | n/a (bugs) | `FightResult` left both fight stats null on a knockout, `DeadPlayer` returned the survivor | **Fixed** |
| 4 | Dead vs stunned | No life status existed | **Fixed**: `PlayerLifeStatus` (Alive, Stunned, Dead) on `Player`, with an EF migration (`AddPlayerLifeStatus`) |
| 5 | Kill vs Stun attack types | `Player.AttackTypes` never read, never settable | **Fixed**: third combat prompt (attack type); Stun is weaker and non-lethal |
| 6 | Defensive counters | Stored on the player, never used | **Fixed**: Override adds Strength and makes the defender lethal even against a stun |
| 7 | Hit-and-run avoids identification, push hard flees identified | No notion of identification | **Fixed**: `FightResult.AttackerIdentified` |
| 8 | Info after combat (death, survivors, witnesses) | `ApplyEndFightConditions` was a `Console.WriteLine`; no game logs | **Fixed**: `CombatAftermath` produces private game logs per the spec rules |
| 9 | Sanity weakens Strength | Flat -1 below an arbitrary 50 | **Partly**: unchanged and now covered by tests; numbers need a decision |
| 10 | Hit-and-run "gap too low" / push hard "losing inevitable" | Any deficit / single-blow check | **Open**: kept, documented as assumptions |
| 11 | Weapons and public/secret modifiers | None | **Open**: no weapon concept exists |
| 12 | Vigilance / surprise, cultists faking weakness | None (code TODO: "ask what vigilance means") | **Open**: spec itself undefined |
| 13 | Ghost chat for dead players | None | **Open** |
| 14 | n/a | `CombatStances` duplicated `AttackerStances`; `ChooseDefensiveCounterApp` injected its dependency twice; `DefensiveCounters` doc was `???` | **Fixed** |

## Numbers the implementation chose (see `CombatConstants`)

Stun -1 Strength (minimum 1), Override +1 Strength, To the end +1 Strength, Health lost on a Toughness break equals the opponent's Strength. All are listed as **[assumed]** in `docs/combat.md`.

## Tests added (all `Given_When_Then`, one assert each)

In `tests/Unit/Features/Actions/General/Combat`:

- `CombatResolutionTest`: rounds, exact-zero death, simultaneous knockout, persistence of Health and status, Toughness carry-over.
- `CombatAttackTypesTest`: Stun and Kill strength, counters, who dies vs who is stunned.
- `CombatStancesTest`: hit-and-run, push hard and to the end, including identification.
- `CombatStatsTest`: toughness per role (all twelve roles), unarmed Strength, sanity.
- `CombatAftermathTest`: death notices, health reports, witness proof privacy.

In `tests/Integration/.../CombatActionAppTest`: attack type prompt, Health persisted after an attack, a Kill fight leaves a dead combatant, a Stun fight leaves nobody dead, the defender receives a log.

## Behaviour change to be aware of

The combat action now has a **third prompt (attack type)** after the player and stance prompts. Clients must send three target screens. The frontend client needs regenerating with orval.
