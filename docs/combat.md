# Combat

Rules marked **[assumed]** are values or interpretations the implementation had to pick because the original text did not say. They live in `CombatConstants` or are noted in code, and need a design decision. Anything marked **[not implemented]** is still open. See `docs/combat-spec-vs-code.md` for the audit behind this revision.

## Combat core

- Deterministic outcomes.
- Each combatant has two stats:
  - **Toughness**: how much Strength must be accumulated before Health is lost.
    - Weak (8): Chaplain, Scholar, Doctor
    - Tough (15): Marine, Brute, Sapper
    - Normal (12): all others
  - **Health**: 100 (full) to 0 (down).
- Resolution sequence:
  1. Combat resolves in rounds.
  2. Each round, both sides simultaneously apply their Strength. The base attack of an unarmed combatant is their Toughness. Strength is also affected by modifiers (see below). **[not implemented: weapons]**
  3. Strength reduces the opponent's Toughness.
  4. If a combatant's Toughness drops to 0 or below they lose Health and their Toughness is restored to full for the next round. **[assumed]** The Health lost is the opponent's Strength.
  5. Combat continues until one side's Health reaches 0. Reaching exactly 0 is down. If both sides reach 0 in the same round, both are down and nobody is the winner. **[assumed]**
- Persistence:
  - Toughness depletion carries between rounds of the same combat.
  - At the end of combat, Toughness is full again for every combatant (it only exists during a fight).
  - Only Health damage, and being dead or stunned, persist outside combat.

## Combat options

- **Attack types** (chosen by the attacker)
  - **Kill**: baseline Strength, lethal at 0 Health.
  - **Stun**: weaker Strength (**[assumed]** -1, minimum 1), incapacitates at 0 Health instead of killing.
- **Defensive counters** (chosen in advance by the defender, default is the stun counter)
  - **Default**: counters in kind, so it is not lethal against a stun.
  - **Override** (kill counter): adds Strength (**[assumed]** +1) and is lethal even against a stun, so a non-lethal attacker risks dying.
- **Who dies and who is stunned** at 0 Health depends on whether the opponent's blows were lethal. The attacker's blows are lethal when the attack type is Kill. The defender's blows are lethal when the attack is Kill or when they chose Override. A combatant taken to 0 by lethal blows is dead, otherwise stunned. **[assumed reading of the original text]**
- **Stances** (chosen by the attacker)
  - **Hit and run**: disengages immediately, before any damage, if the attacker is weaker than the defender. The attacker avoids damage and is not identified. **[assumed]** "Strength gap too low" means any deficit at all. **[open]** Should there be a margin?
  - **Push hard**: the attacker continues until the next blow would kill them, then flees. If no kill was achieved the attacker is identified. **[assumed]** The test is "the defender's Strength would take all my remaining Health in one hit" rather than a true "defeat is inevitable" projection. **[open]**
  - **To the end**: small Strength boost (**[assumed]** +1, attacker only). The attacker never flees: if they lose they die, or are stunned if the defender's blows were not lethal.

## Hidden modifiers

- Role base Toughness: fighters are harder to wear down, support roles weaker. Implemented (see tiers above).
- Sanity: depressed combatants are weaker. **[assumed]** Sanity below 50 gives -1 Strength, minimum 1. The threshold and amount need a decision.
- Vigilance / surprise: vigilance prevents surprise but costs TP; targeted vigilance is stronger against one target and weaker against others. **[not implemented, and the intended mechanic is undefined: what is TP, what is surprise?]**
- Cultists faking weakness. **[not implemented]**
- Weapons and other public or secret Strength modifiers. **[not implemented: no weapon concept exists on items yet]**

## Information after combat

Implemented in `CombatAftermath`, delivered as private game logs.

- **Defender dies**: the dead player is told only "You died." No fight summary is made public, so the attacker controls the narrative. A dead attacker is also only told they died.
- **Defender is stunned**: told they were knocked out. **[assumed]**
- **Defender survives, alone with the attacker**: the only public fact is the post-combat Health of both, shown to attacker and defender. Nothing proves who attacked first, so either can claim self-defense. **[assumed]** Health is shown as a number out of 100.
- **Witnesses** (everyone else in the room): each gets hard proof of who attacked first. The proof is private, never shared mechanically with the crew, and the witness may lie when reporting. If the defender survived, witnesses also see the Health states.
- Dead players can still take part in post-death chat (ghost mode) without any privileged info. **[not implemented]**
- Loyal and cultist witnesses may lie to shield or frame someone. Lying is a human decision, not a mechanic.

## Open questions for the design owner

1. How much Health is lost when Toughness breaks: the opponent's Strength, a fixed amount, or Strength minus Toughness? (Currently the opponent's Strength.)
2. Margins: hit-and-run "gap too low", and push-hard "losing is inevitable".
3. Numbers for Stun, Override, To the end and Sanity (all currently +/-1).
4. What are Vigilance, TP and Surprise?
5. How do weapons feed Strength, and what do Kill and Stun mean for a weapon?
6. What can a dead or stunned player still do, and how does a stunned player recover?
7. Simultaneous knockout: both down, no winner. Is that intended?
