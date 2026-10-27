# Combat

## 1. General principles

- Combat outcomes are deterministic. The only random element is whether the attacker is identified in certain Hit-and-run outcomes.
- The attacker must be in the same room as the defender.
- The system resolves each combat automatically. It uses the defender's combat reactions, set in advance, and the choices the attacker makes when attacking.
- The defender fights with the combat reactions active at the time of the combat.
- The Chaplain can never attack humans (other characters or Inuits).

## 2. Combatant statistics

### Health

- Ranges from 0 to 100.
- Shown publicly only in 20% increments (five increments). No one knows the exact value, not even the player.

### Hope (loyals)

- Ranges from 0 to 100.
- Never public. Known secretly by the player only in 20% increments (five increments), never the exact value.

### Hope (acolytes)

- Has no mechanical effect, and acolytes don't die at 0 Hope.
- It is tracked like a loyal's Hope, only so the acolyte knows what state he should claim to be in and possibly which penalty to fake.

### Vigilance

- Each character decides whether to be vigilant or not. It is a persistent setting that can be changed at any time. This setting is secret.
- Being vigilant reduces hope by 1 per tick. Acolytes also have +2 on their secret rogue gauge per tick for being vigilant.
- Without vigilance, a character is surprised when attacked.
- The Sentry always has vigilance for free.

### Toughness

- Fixed for each role, publicly known, never changes.
- Three tiers: 3 (Chaplain, Scholar, Doctor), 5 (Marine, Brute, Sapper), 4 (all other roles).
- Each time accumulated damage equals Toughness, Health drops by 1.
- Leftover damage below Toughness carries over within a combat and is discarded when the combat ends. Only Health loss persists.

### Strength

- Damage dealt with each strike.
- Here is the basic strength of each weapon. The strongest available ranged weapon is used at range, and then the strongest available melee weapon in melee. The defender automatically uses his strongest available weapons. The attacker uses the strongest among the weapons he chose to use:

| Weapon | Type | Inside | Outside |
|---|---|---|---|
| Unarmed | Melee | Toughness × 5 | Toughness × 5 |
| Knife | Melee | 40 | 40 |
| Pistol | Ranged | 100 | 100 |
| Sword | Melee | 30 | 60 |
| Musket | Ranged | 75 | 175 |

- Specialists do more damage with their weapon of choice: Brute +15 in unarmed combat, Marine +15 with the sword and +25 with the musket, Ranger +25 with the musket. These are different basic damages for the specialists.
- Here are the modifiers for health and hope. Health modifiers apply only to melee damage; they are fixed at the start of the combat. Damage taken during the combat doesn't change them. Hope modifiers apply to both ranged and melee but only to loyals; acolytes can fake a strength penalty if they want to but they are not afflicted by one.

| Health (melee only) | Strength Penalty | Hope (loyals only) | Strength Penalty |
|---|---|---|---|
| 1-20% | -20% | 1-20% | -30% |
| 21-40% | -15% | 21-40% | -10% |
| 41-60% | -10% | 41-60% | None |
| 61-80% | -5% | 61-80% | None |
| 81-100% | None | 81-100% | None |

- +10% for the stance "to the end", -20% for the stance "hit-and-run". These apply only during the rounds, never to the surprise attack or the closing-in strike. -20% for all stun damages.
- If a character is weakened, his strength is reduced by 30%. Causes of weakness include poison, laudanum, curse and, for acolytes, exorcism. Causes of weakness do not stack: every player is either weakened or not.
- All modifiers (Health and Hope penalties, attack stance and stun damage, weakening) apply directly to the basic damage. Strength can fall to 0 because of cumulative effects, and players might not know it (if they don't know they are cursed, for example). Strength cannot become negative. If an attacker has 0 Strength when combat rounds are reached, he breaks off his attack regardless of his stance.
- Explosives are not weapons and they are not used during combat.

## 3. Damage types

- **Kill damage:** lethal when Health reaches 0.
- **Stun damage:** incapacitates when Health reaches 0.
- At the end of the combat, 75% of the Health lost to stun damage is cancelled.
- Weapon damage is always kill damage.
- Stun damage is always unarmed.
- Unarmed damage can be kill or stun.

## 4. Attacker choices

When he attacks, the attacker chooses:

- the attack type (kill or, if unarmed, stun);
- what weapons, both ranged and melee, are used for this combat. If no ranged weapon is used, it is a melee attack with no distance or closing-in involved;
- the faked Hope penalty, for acolytes;
- the attack stance among the three following options:

### Push hard (default)

- The attacker stops attacking if, at the end of a round, his Health reaches 20% or less and he would lose the combat. The attacker can still be killed if a single strike from the defender takes his Health from above 20% to 0.

### To the end

- +10% Strength during the rounds (not during surprise attack and closing in).
- The attacker fights until his health or the defender's health reaches 0.

### Hit-and-run

- −20% Strength during the rounds (not during surprise attack and closing in).
- Whether the defender was surprised or not, the system checks after step 1 whether the attacker's Health would drop to 40% or less over the combat. If it would, the combat stops after the surprise attack (or lack of surprise attack if the defender was not surprised), with no closing in and no rounds.
- If it wouldn't, the combat goes ahead in full.

## 5. Combat reactions (defense)

- Each player has an interface which defines their combat reaction in reaction to an attack.
- For each other player and each attack type (stun, kill), the defender chooses whether to counter with kill damage or stun damage. If the counter is kill damage, also set which weapons to use. That makes up to 44 settings. The choice of weapons are either "strongest owned" or "strongest owned and not concealed".
- Acolytes also set, for each player and each attack type, the faked Hope penalty they apply. That makes 22 additional settings for a total of up to 66 for acolytes.
- Defenders have no stance and cannot break off.

### Default settings

- Kill damage against kill attacks.
- Stun damage against stun attacks.
- Concealed weapons allowed against kill attacks, not allowed against stun attacks.
- Acolytes: against a stun, they fake the penalty matching their tracked Hope; against a kill, they fight at full Strength.

## 6. Order of a combat

### Before the combat

- All modifiers are fixed.
- An attack with a ranged weapon starts at a distance.
- An attack with a melee weapon or bare hands starts next to the target.

### Step 1: surprise attack

- If the defender is surprised, the attacker strikes once at his Strength, with no counter. If the attacker is using a melee attack (he decided not to use ranged weapons if he owns one), the damage of the surprise attack is doubled.

### Step 2: closing in

- A melee attack has no closing in.
- Against a ranged attacker, a defender with no ranged weapon always closes in.
- When both fighters have ranged weapons:
  - The attacker closes in if that gives him a better result than ranged rounds.
  - If the attacker doesn't close in, the defender makes the same choice.
  - If both fighters have better results without closing in, they move to step 3 with their ranged weapons.
  - A better result means winning with the most Health left, or losing with the opponent left at the lowest Health.
- Any fighter who closes in on an opponent holding a ranged weapon takes one strike at that opponent's Strength, with no counter.
- After closing in, ranged weapons are unavailable for both fighters and they must use melee weapons or bare hands.

### Step 3: rounds

- Each round, the attacker strikes first. If the defender is not at 0 Health, he strikes back.
- The rounds use ranged weapons if both fighters have one and neither closed in. Otherwise they use melee weapons, bare hands included.
- The combat continues until one fighter reaches 0 Health, or until the attacker breaks off under his stance. When the attacker breaks off, for any reason, the defender gets no free strike in return.

## 7. End of combat

- Leftover Toughness damage is discarded. Only Health loss persists.
- 75% of the Health lost to stun damage is cancelled, rounded up.

## 8. Identification and information

### Hit-and-run that stops early

- Stopped after the surprise attack: the defender identifies the attacker with 20% probability.
- Stopped with no damage dealt because the defender is not surprised: the defender identifies the attacker with 60% probability.
- The defender always knows he was targeted by a Hit-and-run, even when the attacker isn't identified or when no damage was dealt.

### Combats that take place

- The defender identifies the attacker automatically, unless the defender is killed.

### Other rules

- A dead defender gets no information except the fact of his death.
- Identification is private to the defender.
- Witnesses (in the same room according to Meta file) get private, hard proof of who started the combat and which weapons were used by each fighter. They may tell the truth or lie.
- The only public trace of a combat is the change in Health increments.
