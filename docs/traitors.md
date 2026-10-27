# Traitors

Hidden-allegiance layer of the game. Each player is either **Loyal** or **Acolyte**.

Status legend: **Fixed** = no more major rework, only tuning. **Open** = still moving or undefined.

## Allegiance (Fixed, implemented)

- 12 players per game, one per role, always (`GameSettings.RequiredPlayerCountToStartGame`).
- Exactly **2 acolytes**, the other 10 are loyal (`AllegianceConstants.AcolyteCount`).
- The 2 acolytes are a uniform pick among the roles that can be acolytes. The **Chaplain can never be an acolyte**, no other role is excluded (`AllegianceConstants.RolesThatCanNeverBeAcolyte`).
- Public knowledge: the number of acolytes, and that the Chaplain is loyal.
- Secret: who the acolytes are. `OtherPlayerDto` has no allegiance field, so another player's allegiance cannot leak through the API.
- Allegiance never changes during a game (no conversion), and is not revealed on death.
- Acolytes may fight each other; no action depends on allegiance yet.

### How a player learns their allegiance

- Stored as `Player.IsAcolyte`, the single source of truth. Drawn in `PlayerFactory.AssignAcolytes`: players are created in shuffled-role order, so taking the first eligible two is already a uniform pick (no extra random draw).
- At game creation `AllegianceAnnouncer` writes one private `GameLog` per player:
  - loyal: `You are loyal.`
  - acolyte: `You are an acolyte. Your fellow acolyte: <username> (<role>).`
  The fellow acolyte is only ever given by this log; no DTO carries it.
- Permanent reminder: `GET /game/me` (`GetMyPlayerRequest`) returns `IsAcolyte`. The frontend shows "Loyal" or "Acolyte" from it.
- The dramatic announcement (text and emblematic image) is frontend-only and already prepared by the game designer.

### DTOs

- `OtherPlayerDto` (`Id`, `Role`, `HealthIncrement`): the whitelist of what a player may know about another player. Every field is written out on purpose; do not add exact health, hope, room, inventory or allegiance without deciding it is public. Health in 20% increments is public (docs/combat.md).
- `GetMyPlayerResponse` (`Id`, `Role`, `LifeStatus`, `IsAcolyte`): only returned to the player themselves, from the session's `PlayerId`.

## Victory conditions (Open)

Not defined yet.

## Scans, pings and gauges (Open)

Not documented yet. The designer is finalizing the list of scans (several were modified or added), expected to be fixed within days. This section will be filled from that list.

## Known gaps

- `GetOtherPlayers` takes the `PlayerId` from the session without checking the game, same as before this change.
