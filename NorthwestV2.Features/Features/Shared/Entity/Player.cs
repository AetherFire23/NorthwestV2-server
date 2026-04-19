using System.ComponentModel.DataAnnotations.Schema;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

namespace NorthwestV2.Features.Features.Shared.Entity;

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

    public int ActionPoints { get; set; } = 8; // default 8

    public int Health { get; set; } = INITIALIZATION_HEALTH;
    public int Sanity { get; set; } = INITIALIZATION_HEALTH;

    /// <summary>
    /// How much strength must be accumulated before health is lost.
    /// Depends per-role.
    /// </summary>
    public required int BaseToughness { get; set; }

    public DefensiveCounters DefensiveCounter { get; set; } = DefensiveCounters.DefaultVsStun;

    public AttackTypes AttackTypes { get; set; } = AttackTypes.Kill;

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

    public bool RoomHas(ItemBase item)
    {
        bool roomHasItem = this.RoomHas(item);

        return roomHasItem;
    }

    public void SwapItemBetweenPlayerOrRoom(ItemBase transferredItem)
    {
        bool neitherPlayerNorHisRoomHaveTheItemToTransfer =
            !HasItem(transferredItem)
            && !Room.Inventory.HasItem(transferredItem);

        if (neitherPlayerNorHisRoomHaveTheItemToTransfer)
        {
            throw new Exception($"Neither player nor inventory contain the target item. : " +
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

    /// <summary>
    /// Strength = Toughness + Weapon + Various Modifiers
    ///  + Weakness caused by Sanity 
    /// + Stance bonus ( ToTheEnd ) 
    /// </summary>
    /// <returns></returns>
    public int CalculateStrength(bool isAttacker, AttackerStances attackerStance)
    {
        // TODO: Clarify at what Threshold the sanity weakness applies
        // initial strength is base toughness. 
        int strength = this.BaseToughness;

        if (this.Sanity < 50)
        {
            // TODO: Clarifiy what is the minimum strength possible
            strength = Math.Max(1, strength - 1); // -1 replace by constant for sanity decrease
        }

        if (attackerStance == AttackerStances.ToTheEnd && isAttacker)
        {
            strength += 1;
        }

        return strength;
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

    // May add services 
    public PlayerTempFightStats GetPlayerTempFightStats(bool isAttacker, AttackerStances attackerStance)
    {
        PlayerTempFightStats player1Strength =
            new PlayerTempFightStats(this.CalculateStrength(isAttacker, attackerStance), this.BaseToughness,
                this.Health,
                this.BaseToughness, attackerStance)
            {
                Player = this
            };

        return player1Strength;
    }

    public override string ToString()
    {
        return this.Role.ToString();
    }
}