using ERP.Testing.Domain.Dummies;
using JetBrains.Annotations;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Features.GameStart.Domain.RoleInitializations;
using NorthwestV2.Features.Features.Shared.Entity;

namespace ERP.Testing.Domain.Features.Actions.General.Combat;

[TestSubject(typeof(CombatAction))]
public class CombatActionTest
{
    private const int ANY_LOW_TOUGHNESS = 3;
    
    private readonly CombatAction _combatAction;
    public CombatActionTest()
    {
        _combatAction = new CombatAction();
    }

    [Fact]
    public void GivenPlayerAndOtherPlayerInSameRoom_WhenGettingAvailability_ThenCanSeeOtherPlayer()
    {
        Game game = new Game();
        Room room = TestPlayers.CreateTestRoom(RoomEnum.Armory, game);
        Player caster =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.Engineer, room, game);
        Player target =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.QuarterMaster, room, game);

        ActionWithTargetsAvailability avail = _combatAction.DetermineAvailability(caster, [target]);

        Assert.True(avail.TargetSelectionPrompts.First().ValidTargets.First().TargetId == target.Id);
    }

    [Fact]
    public void GivenPlayerWithVeryLowToughness_WhenAttackingOtherPlayer_ThenLosesFight()
    {
        Game game = new Game();
        Room room = TestPlayers.CreateTestRoom(RoomEnum.Armory, game);
        Player caster =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.Engineer, room, game);
        Player target =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.QuarterMaster, room, game);
        // Setting base toughness to 1 so it's always losing 
        caster.BaseToughness = 1;

        FightResult action = _combatAction.MakeTwoPlayerFightTogether(caster, target, AttackerStances.ToTheEnd);

        Assert.True(action.IsWinner(target));
    }

    [Fact]
    public void GivenTwoPlayersWithSuperiorToughness_WhenAttackingOtherPlayer_ThenWins()
    {
        Game game = new Game();
        Room room = TestPlayers.CreateTestRoom(RoomEnum.Armory, game);
        Player caster =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.Engineer, room, game);
        Player target =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.QuarterMaster, room, game);

        // Increase toughness so caster most decidedly wins
        caster.BaseToughness += 2;

        FightResult action = _combatAction.MakeTwoPlayerFightTogether(caster, target, AttackerStances.ToTheEnd);

        // TODO: the real assertion 
        Assert.True(Equals(action.SurvivingPlayer.Player, caster));
    }

    [Fact]
    public void
        GivenAttackingPlayerWithHitAndRunStanceAndInferiorToughness_WhenAttackingStrongerPlayer_ThenFleesCombat()
    {
        Game game = new Game();
        Room room = TestPlayers.CreateTestRoom(RoomEnum.Armory, game);
        Player caster =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.Engineer, room, game);
        Player target =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.QuarterMaster, room, game);
        // Increase toughness so caster most decidedly wins
        caster.BaseToughness = 1;

        FightResult action = _combatAction.MakeTwoPlayerFightTogether(caster, target, AttackerStances.HitAndRun);

        // TODO: the real assertion 
        Assert.True(action.FightExitType == FightExitType.EarlyExittedBecauseOfHitAndRun);
    }

    [Fact]
    public void
        GivenAttackerWithPushHardStanceAndStrongerDefender_WhenAttacking_ThenFleesCombat()
    {
        Game game = new Game();
        Room room = TestPlayers.CreateTestRoom(RoomEnum.Armory, game);
        Player caster =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.Engineer, room, game);
        Player target =
            TestPlayers.CreateTestPlayer(ToughnessInitializationConstants.NORMAL, Roles.QuarterMaster, room, game);
        caster.BaseToughness = ANY_LOW_TOUGHNESS;

        FightResult action = _combatAction.MakeTwoPlayerFightTogether(caster, target, AttackerStances.PushHard);

        Assert.Equal(FightExitType.ExittedBecauseOfPushHard, action.FightExitType);
    }

    // TODO:
    // End conditions of the fight ! 
}