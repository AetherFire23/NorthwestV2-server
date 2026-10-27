using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

namespace NorthwestV2.Features.Shared.Entity;

/// <summary>
/// How a player reacts when one specific opponent attacks him with one specific attack type.
/// Only changed settings are stored: a missing row means <see cref="CreateDefault"/>.
/// </summary>
public class CombatReaction : EntityBase
{
    public Guid OwnerId { get; set; }
    public virtual Player Owner { get; set; } = null!;

    /// <summary>
    /// The attacking player this reaction applies to. No navigation on purpose: the owner is the only side that reads it.
    /// </summary>
    public Guid OpponentId { get; set; }

    public AttackTypes AttackType { get; set; }

    /// <summary>
    /// Kind of damage dealt back. A stun counter is always unarmed.
    /// </summary>
    public AttackTypes Counter { get; set; }

    /// <summary>
    /// Only used when <see cref="Counter"/> is <see cref="AttackTypes.Kill"/>.
    /// </summary>
    public WeaponPolicies WeaponPolicy { get; set; }

    /// <summary>
    /// Only used for acolytes.
    /// </summary>
    public FakedHopePenalties FakedHopePenalty { get; set; }

    /// <summary>
    /// docs/combat.md defaults: counter in kind; concealed weapons only against a kill;
    /// acolytes fake their tracked Hope against a stun and fight at full Strength against a kill.
    /// </summary>
    public static CombatReaction CreateDefault(Player owner, Guid opponentId, AttackTypes attackType)
    {
        bool isKill = attackType == AttackTypes.Kill;

        CombatReaction reaction = new CombatReaction
        {
            Owner = owner,
            OwnerId = owner.Id,
            OpponentId = opponentId,
            AttackType = attackType,
            Counter = attackType,
            WeaponPolicy = isKill ? WeaponPolicies.StrongestOwned : WeaponPolicies.StrongestOwnedNotConcealed,
            FakedHopePenalty = isKill ? FakedHopePenalties.None : FakedHopePenalties.MatchTrackedHope,
        };

        return reaction;
    }
}
