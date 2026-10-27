using System.ComponentModel.DataAnnotations.Schema;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

namespace NorthwestV2.Features.Shared.Entity;

/// <summary>
/// Represents a user playing in a game instance. A player is tied to:
/// - A <see cref="User"/> (account)
/// - A <see cref="Game"/> (game session)
/// - A <see cref="Room"/> (current location)
/// - A <see cref="Role"/> (character class)
/// - An <see cref="Inventory"/> (items carried)
///
/// Players have health, hope, and action points. In combat, every BaseToughness worth of damage
/// costs 1 Health (docs/combat.md). Each role has different starting stats.
/// </summary>
public class Player : EntityBase
{
    [NotMapped] [NonSerialized] public const int INITIALIZATION_HEALTH = 100;

    public Guid UserId { get; set; }

    public virtual required User User { get; set; }

    public required Roles Role { get; set; }

    public Guid GameId { get; set; }
    public required virtual Game Game { get; set; }

    public Guid RoomId { get; set; }
    public required virtual Room Room { get; set; }

    public virtual Inventory Inventory { get; set; }

    public ICollection<GameLog> GameLogs { get; set; } = [];

    public int ActionPoints { get; set; } = 8; // default 8

    /// <summary>
    /// 0 to 100. Never shown exactly, not even to the player: only in 20% increments.
    /// </summary>
    public int Health { get; set; } = INITIALIZATION_HEALTH;

    /// <summary>
    /// 0 to 100, secret. For acolytes it has no mechanical effect: it only tells them what state to claim.
    /// </summary>
    public int Hope { get; set; } = INITIALIZATION_HEALTH;

    /// <summary>
    /// How much strength must be accumulated before health is lost.
    /// Depends per-role.
    /// </summary>
    public required int BaseToughness { get; set; }

    public PlayerLifeStatus LifeStatus { get; set; } = PlayerLifeStatus.Alive;

    /// <summary>
    /// Allegiance, fixed at game creation. Secret: only the player themselves may ever be told. See docs/traitors.md.
    /// Never changes during a game (no conversion) and stays secret after death, so it is not tied to
    /// <see cref="LifeStatus"/>. Acolytes may attack each other (it can serve as a cover), so combat never checks
    /// allegiance between fighters; it only decides whether a Hope penalty is real or faked.
    /// </summary>
    public bool IsAcolyte { get; set; }

    /// <summary>
    /// Secret, persistent setting. A player who is not vigilant is surprised when attacked.
    /// The per-tick Hope (and acolyte rogue gauge) cost is not implemented: there is no tick system yet.
    /// </summary>
    public bool IsVigilant { get; set; }

    /// <summary>
    /// -30% Strength. Causes (poison, laudanum, curse, exorcism) do not stack and are not implemented yet.
    /// </summary>
    public bool IsWeakened { get; set; }

    /// <summary>
    /// Only the reactions the player changed; see <see cref="GetCombatReaction"/> for the defaults.
    /// </summary>
    public ICollection<CombatReaction> CombatReactions { get; set; } = [];

    /// <summary>
    /// The Sentry always has vigilance for free.
    /// </summary>
    public bool IsEffectivelyVigilant => Role == Roles.Sentry || IsVigilant;

    public CombatReaction GetCombatReaction(Guid opponentId, AttackTypes attackType)
    {
        CombatReaction reaction = CombatReactions
                                      .FirstOrDefault(x => x.OpponentId == opponentId && x.AttackType == attackType)
                                  ?? CombatReaction.CreateDefault(this, opponentId, attackType);

        return reaction;
    }

    public void SetCombatReaction(Guid opponentId, AttackTypes attackType, AttackTypes counter,
        WeaponPolicies weaponPolicy, FakedHopePenalties fakedHopePenalty)
    {
        CombatReaction? existing = CombatReactions
            .FirstOrDefault(x => x.OpponentId == opponentId && x.AttackType == attackType);

        CombatReaction reaction = existing ?? CombatReaction.CreateDefault(this, opponentId, attackType);
        reaction.Counter = counter;
        reaction.WeaponPolicy = weaponPolicy;
        reaction.FakedHopePenalty = fakedHopePenalty;

        if (existing is null)
        {
            CombatReactions.Add(reaction);
        }
    }

    public bool Has<T>() where T : ItemBase
    {
        bool hasItem = Inventory.Has<T>();

        return hasItem;
    }

    public bool HasItem(ItemBase item)
    {
        bool hasItem = Inventory.Items.Contains(item);

        return hasItem;
    }

    public void SwapItemBetweenPlayerOrRoom(ItemBase transferredItem)
    {
        bool neitherPlayerNorHisRoomHaveTheItemToTransfer =
            !HasItem(transferredItem)
            && !Room.Inventory.HasItem(transferredItem);

        if (neitherPlayerNorHisRoomHaveTheItemToTransfer)
        {
            throw new Exception($"Either player or room does not the target item. : " +
                                $"Item id: {transferredItem.Id}" +
                                $"Player id: {this.Id}"
            );
        }

        if (Room.Inventory.HasItem(transferredItem))
        {
            this.Inventory.TakeOwnershipAndRemoveFromPreviousOwner(transferredItem);
        }
        else
        {
            this.Room.TakeOwnershipAndRemoveFromPreviousOwner([transferredItem]);
        }
    }

    public ActionTarget ToTarget()
    {
        ActionTarget actionTarget = new ActionTarget()
        {
            Name = this.Role.ToString(),
            TargetId = this.Id,
        };

        return actionTarget;
    }

    public override string ToString()
    {
        return this.Role.ToString();
    }
}