**Setting**

This file contains information about statuses, rooms and random events

**Statuses**

**1. Status Scores**

- **Health**: Numeric score from **0 to 100**. At 0 → immediate death.
- **Hope**: Numeric score from **0 to 100**. At 0 → immediate death.
- Both have **hard caps at 100** and cannot go below 0.

These are **status scores**: they are continuous values, tracked point by point.

**2. Status Levels**

Some needs (hunger, fatigue) are tracked as **levels** instead of scores. These are capped at defined ranges and never drop below 0.

- **Hunger** (0–4):
  - 0: Well-fed (no effect)
  - 1: Hungry (−1 Hope per tick)
  - 2: Famished (−2 Hope per tick, −1 TP per tick)
  - 3: Starving (−3 Hope per tick, −2 TP per tick)
  - 4: Immediate death by starvation
- **Fatigue** (0–4):
  - 0: Rested (no effect)
  - 1: Tired (no effect)
  - 2: Weary (−1 TP per tick)
  - 3: Exhausted (−2 TP per tick)
  - 4: Collapsed (loses all TP for one full day; next day resumes at level 3, Exhausted)

**3. Sickness**

- **Sickness Score**: Starts at 0, has **no upper cap**.
- At **100 or more**: the player becomes **Sick**.
- Sick persists until the sickness score is reduced all the way back to **0**.
- While Sick, apply each tick:
  - −2 TP
  - −2 Health
  - −3 Hope

**Secrecy**: Only the Doctor’s **Diagnosis** action can gauge sickness. It returns an approximate band, not an exact value.

**Warmth**: The current warmth level always modifies sickness progression.

- Example modifiers:
  - No clothes outside: +60 sickness per tick.
  - Uniform outside: +30 per tick.
  - No clothes inside: +20 per tick.

**4. Daily Needs**

Every in-game **day is 4 ticks**. These ticks represent both the daily rhythm and the resolution of needs.

- A player may **eat/sleep at the last tick of one day and the first tick of the next**, fulfilling two days’ needs consecutively.

**Food**

- Must consume **1 meal per day** (costs 1 TP).
- Daily resolution:
  - If **no meal**: Hunger +1.
  - If **≥2 meals**: Hunger −1 (to a minimum of 0).

**Sleep**

- Must rest **once per day**.
- Daily resolution:
  - If **no rest**: Fatigue +1.
  - If **≥2 rests**: Fatigue −1 (to a minimum of 0).

**5. Clothing**

- **Inside the ship**: Uniform required.
- **Outside (deck/expeditions)**: Coat required.

Penalties:

- No clothes: −3 Hope per tick, +20 sickness per tick inside / +60 per tick outside.
- Uniform outside instead of coat: +30 sickness per tick.
- Dirty clothes: −2 Hope per tick (no sickness effect).

**6. Resolution Order (per tick)**

- Apply **environmental sickness changes** (warmth + clothing).
- Apply **status effects**:
  - TP reduction (worst penalty only – TP loses are not cumulative).
  - Health and Hope drains (stacking across all sources).
- Check **thresholds**:
  - Health = 0 → death.
  - Hope = 0 → death.
  - Sickness ≥ 100 → sick.
  - Sickness = 0 → cured.
  - Hunger = 4 → death.
  - Fatigue = 4 → collapse.

**Toxicity (Ration Poisoning Mechanic)**

- **Toxicity Score**: each player tracks this value.
  - Floor = **5** (never goes lower).
  - No upper cap.
- **When a ration (uncooked can) is eaten**:
  - The player immediately **increases their sickness score** by an amount equal to their **current Toxicity score**.
  - Their **Toxicity score increases by +30** (after the sickness increase).
- **At the start of every tick**:
  - Toxicity naturally decreases by **2**, but never below 5.

**Rooms**

Each room is either *intact* or *damaged* — no intermediate severity.

  - Damaged room = unusable. Any action tied to that room cannot be performed.
  - Damaged corridor = movement cost doubled.
  - Damaged stairs = movement cost tripled.
- **Crisis threshold**: If ≥ 12 rooms are damaged simultaneously, the ship breaks apart and sinks → game loss condition.

**Room Categories**

**Outside Rooms**

- **Forecastle, Main Deck, Quarterdeck**: Count as “outside” for all mechanics.
- **Mess**: Counts as “outside” only for **weapons use** (combat effects, visibility of attacks).

**Inside Rooms**

- All other rooms are “inside”.

**Locked Rooms**

**Brig**

- Starts empty.
- Requires **Prison Key** (Captain) for jailing/freeing.
- If door is broken, room is unsecured.

**Armory**

- Starts empty.
- Requires **Armory Key** (Quartermaster).
- Stores and secures heavy weapons.

**Magazine**

- Starts with **unlimited gunpowder**.
- Requires **Magazine Key** (Quartermaster).

**Mess Hall – Hope Effects**

- **Feast Boost**:
  - When **multiple players consume hope-boosting food in the same tick** → hope gain is amplified for all.
  - Thematically: communal morale rises.
- **Elsewhere Consumption**:
  - Food consumed outside the mess loses part (or all) of its hope benefit.
  - Survival nutrition still applies, but morale is dulled.

**Locked Rooms**

- **Brig**
  - **No storage** permitted.
  - Prisoners cannot access or conceal items inside.
  - Its only function is confinement.
- **Magazine**
  - Contains **gunpowder**.
  - **No other storage** allowed.
  - Breaking it = catastrophic suspicion (anyone can now access powder freely).
- **Armory**
  - **Weapons only**: knives, swords, pistols, muskets (+upgraded versions).
  - **Explosives only**: grenades and dynamites.
  - All other items/resources forbidden.
  - Provides secure circulation control — breaking it means weapons permanently free to circulate.

**Other Resource Rooms**

- **Coal Room**
  - Stores **coal supply only**.
  - Unlimited capacity, but strictly fuel.
- **Food Room**
  - Stores **cans, flour, liquor only**.
  - Cannot hold tools or weapons.
  - Poisoning risk always applies at pick-up. (cannot poison stock)
- **Hold**
  - **Unlimited storage capacity**.
  - Stores **scrap supply** (default stockpile).
  - May also store *any* item type (tools, weapons, keys, etc.).
  - High-traffic, high-suspicion area if things go missing.

**Locked Door Cycle**

- **Breaking**
  - Any locked door can be broken.
  - Effect: room is permanently accessible until repaired.
  - Old lock immediately becomes useless → its key is voided and removed from play.
- **Repairing**
  - High-TP craft (Engineer/Sapper domain).
  - Restores the lock **and** produces a **new functional key**.
- **New Key Ownership**
  - The repairer receives the new key upon completion.

**Random events**

**Breakage**

A chosen room becomes **damaged**.

**Fire**

A fire breaks out in a chosen room. At first, it has no effect — but if not suppressed within a short delay, the room becomes **damaged**. Once damaged, the fire remains active and begins to spread to adjacent rooms over time. Each new room follows the same sequence.

**Unrest**
**Effect:** A surge of fear, rumor, or quarrel spreads among the crew. By default, the event immediately inflicts **−2 Hope on all players**. This loss is modest and manageable; it requires no coordination to resolve and cannot be prevented.

**Disturbance and Escalation:** If fueled by an acolyte’s **Disturb** action, the event is channeled into **Loathing, Guilt, or Isolation**. These outcomes are always recognizable as acolyte-driven and inflict far heavier consequences than the baseline. If a single channel causes enough cumulative Hope loss, the next time that same channel is fueled it will trigger an **escalated version** with greater damage.

**Codename Rule**
Each fueled unrest event is identified by a unique flavor text (codename) within its channel. A codename may never be repeated. When all minor codenames of a channel are exhausted, that channel can no longer produce minor versions and only escalated versions remain. Escalated versions are also unique, but their pool is sufficient to cover every possible escalation caused by the channel’s cumulative Hope loss.

**Guilt**
**Theme:** Moral scandal corrodes trust. Crew members feel judged, condemned, and unworthy. Hope loss is severe unless the crew confronts the charges.

**Crew reactions:**

- **Scrutiny** – The Chaplain conducts repeated *Confession* actions on different players until several suspicious pings are obtained. Each iteration deepens mistrust and **erodes the whole crew’s Hope**.
- **Redemption** – Instead of pursuing scrutiny, the Chaplain may depart on a prolonged expedition. If successful, this resolves the crisis of Guilt, though his absence hinders inquiry and survival tasks.

**If the Chaplain is dead:**

- *Scrutiny* may be replaced with repeated *Interrogate* actions, but a **much larger number of suspicious pings** is required.
- *Redemption* may be replaced with a collective expedition involving **at least 75% of surviving crew members**.

**Tactical significance:** Guilt forces either mass inquiry or prolonged absence from the ship. Both outcomes can be **instrumental to acolytes** — diverting attention with endless suspicion or covering behavior while many players are away.

**Loathing**
**Theme:** Crew members are seized by disgust at humanity’s destruction of the natural world. They loathe not themselves, but mankind as a whole — its greed, its waste, its scarring of the earth. Hope loss is severe unless the crew channels this hatred into punishment or renunciation.

**Crew reactions:**

- **Scapegoat** – A new player must be jailed for a time. The chosen player must be one of the **production roles**, symbolizing humanity’s exploitation of nature through industry.
- **Austerity** – Instead of scapegoating, the crew may prohibit most production actions for a while. This enforced restraint blunts the hatred, but sharply reduces efficiency.

**Tactical significance:** Loathing functions as a **production hindrance**. Whether through jailing a productive member or stalling collective work, it undercuts the ship’s ability to endure. Acolytes can exploit either path, forcing sacrifice of progress in the name of penance.

**Isolation**
**Theme:** Estrangement spreads through the crew. Members withdraw from one another, trust frays, and cooperation falters. Hope loss is severe unless the crew reacts collectively or individually.

**Crew reactions:**

- **Feast** – A gathering is organized in the Mess where the crew eats and fraternizes. This requires shared participation and food resources, but restores cohesion and mitigates the effects of Isolation.
- **Solitude** – Alternatively, players may avoid sharing rooms, productions, and expeditions with others. This reduces efficiency and inquiry opportunities, but prevents despair from spreading further.

**Tactical significance:** Isolation forces the crew into **spread or gathering**. Either choice creates specific opportunities for acolytes’ rogue actions: dispersed players are more vulnerable to sabotage, while clustered players widen suspect pools.
