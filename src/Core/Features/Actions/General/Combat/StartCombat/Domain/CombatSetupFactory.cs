using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Fixes everything before the combat starts: weapons, basic damages and modifiers of both fighters.
/// </summary>
public class CombatSetupFactory
{
    public CombatSetup Create(Player attacker, Player defender, AttackerChoices choices, bool isOutside)
    {
        CombatReaction reaction = defender.GetCombatReaction(attacker.Id, choices.AttackType);

        Combatant attackerCombatant = CreateAttacker(attacker, choices, isOutside);
        Combatant defenderCombatant = CreateDefender(defender, reaction, isOutside);

        CombatSetup setup = new CombatSetup
        {
            Attacker = attackerCombatant,
            Defender = defenderCombatant,
            Stance = choices.Stance,
            IsDefenderSurprised = !defender.IsEffectivelyVigilant,
        };

        return setup;
    }

    /// <summary>
    /// A stun attack is always unarmed, so the weapon choices are ignored for it.
    /// </summary>
    private static Combatant CreateAttacker(Player attacker, AttackerChoices choices, bool isOutside)
    {
        bool isStun = choices.AttackType == AttackTypes.Stun;
        Weapons? rangedWeapon = isStun ? null : choices.RangedWeapon;
        Weapons meleeWeapon = isStun ? Weapons.Unarmed : choices.MeleeWeapon;

        return CreateCombatant(attacker, rangedWeapon, meleeWeapon, choices.AttackType, choices.FakedHopePenalty,
            isOutside);
    }

    /// <summary>
    /// The defender has no say during the combat: he automatically uses his strongest weapons allowed by his reaction.
    /// A stun counter is unarmed, so he has no ranged weapon and will close in against a ranged attacker.
    /// </summary>
    private static Combatant CreateDefender(Player defender, CombatReaction reaction, bool isOutside)
    {
        bool isStun = reaction.Counter == AttackTypes.Stun;
        bool includeConcealed = reaction.WeaponPolicy == WeaponPolicies.StrongestOwned;

        Weapons? rangedWeapon = null;
        Weapons meleeWeapon = Weapons.Unarmed;

        if (!isStun)
        {
            List<Weapons> ranged = WeaponCatalog.OwnedWeapons(defender, WeaponRanges.Ranged, includeConcealed);
            List<Weapons> melee = WeaponCatalog.OwnedWeapons(defender, WeaponRanges.Melee, includeConcealed);

            rangedWeapon = ranged.Count == 0 ? null : WeaponCatalog.Strongest(defender, ranged, isOutside);
            meleeWeapon = WeaponCatalog.Strongest(defender, [Weapons.Unarmed, ..melee], isOutside);
        }

        return CreateCombatant(defender, rangedWeapon, meleeWeapon, reaction.Counter, reaction.FakedHopePenalty,
            isOutside);
    }

    private static Combatant CreateCombatant(Player player, Weapons? rangedWeapon, Weapons meleeWeapon,
        AttackTypes damageDealt, FakedHopePenalties fakedHopePenalty, bool isOutside)
    {
        int rangedPercent = CombatModifiers.EffectiveHopePenaltyPercent(player, fakedHopePenalty)
                            + (player.IsWeakened ? CombatConstants.WEAKENED_PERCENT : 0)
                            + (damageDealt == AttackTypes.Stun ? CombatConstants.STUN_PERCENT : 0);
        int meleePercent = rangedPercent + CombatModifiers.HealthPenaltyPercent(player.Health);

        Combatant combatant = new Combatant
        {
            Player = player,
            Toughness = player.BaseToughness,
            RangedWeapon = rangedWeapon,
            MeleeWeapon = meleeWeapon,
            RangedBasicDamage = rangedWeapon.HasValue
                ? WeaponCatalog.BasicDamage(player, rangedWeapon.Value, isOutside)
                : 0,
            MeleeBasicDamage = WeaponCatalog.BasicDamage(player, meleeWeapon, isOutside),
            RangedModifierPercent = rangedPercent,
            MeleeModifierPercent = meleePercent,
            DamageDealt = damageDealt,
            Health = player.Health,
        };

        return combatant;
    }
}
