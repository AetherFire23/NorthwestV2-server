using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Unit.Dummies;

public static class TestFighters
{
    public static Player CreateFighter(int toughness, Roles role = Roles.Engineer)
    {
        return CreateFighterIn(RoomEnum.Armory, toughness, role);
    }

    public static Player CreateFighterIn(RoomEnum roomEnum, int toughness, Roles role = Roles.Engineer)
    {
        Game game = new Game();
        Room room = TestPlayers.CreateTestRoom(roomEnum, game);
        Player fighter = TestPlayers.CreateTestPlayer(toughness, role, room, game);

        return fighter;
    }

    /// <summary>
    /// A defender who is vigilant, so the combat starts without a surprise attack.
    /// </summary>
    public static Player Vigilant(this Player player)
    {
        player.IsVigilant = true;

        return player;
    }

    public static Player Carrying(this Player player, params ItemBase[] items)
    {
        player.Inventory.Items.AddRange(items);

        return player;
    }

    public static AttackerChoices Choices(AttackerStances stance, AttackTypes attackType = AttackTypes.Kill,
        Weapons? rangedWeapon = null, Weapons meleeWeapon = Weapons.Unarmed)
    {
        return new AttackerChoices
        {
            Stance = stance,
            AttackType = attackType,
            RangedWeapon = rangedWeapon,
            MeleeWeapon = meleeWeapon,
        };
    }
}
