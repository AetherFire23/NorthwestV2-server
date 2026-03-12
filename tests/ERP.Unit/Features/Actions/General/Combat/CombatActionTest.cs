using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Features.Actions.General.Combat;
using AetherFire23.ERP.Domain.GameStart.RoleInitializations;
using AetherFire23.ERP.Domain.Role;
using ERP.Testing.Domain.Dummies;
using JetBrains.Annotations;

namespace ERP.Testing.Domain.Features.Actions.General.Combat;

[TestSubject(typeof(CombatAction))]
public class CombatActionTest
{
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
        caster.AttackerStance = AttackerStances.ToTheEnd;

        FightResult action = _combatAction.MakeTwoPlayerFightTogether(caster, target);

        // TODO: the real assertion 
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

        FightResult action = _combatAction.MakeTwoPlayerFightTogether(caster, target);

        // TODO: the real assertion 
        Assert.True(Equals(action.Winner, caster));
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
        caster.AttackerStance = AttackerStances.HitAndRun;
        // Increase toughness so caster most decidedly wins
        caster.BaseToughness = 1;

        FightResult action = _combatAction.MakeTwoPlayerFightTogether(caster, target);

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
        caster.BaseToughness = 3;
        caster.AttackerStance = AttackerStances.PushHard;
        
        FightResult action = _combatAction.MakeTwoPlayerFightTogether(caster, target);
        
        Assert.Equal(FightExitType.ExittedBecauseOfPushHard, action.FightExitType);
    }
    
    // TODO:
    // Cend conditions
    
}