using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Actions.General.Combat.StartCombat;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.General.Combat;

/// <summary>
/// Every player starts in the same room, so anyone can attack anyone. Players are picked by role, not creation order.
/// </summary>
[TestSubject(typeof(CombatActionApp))]
public class CombatActionAppTest : NorthwestTestBase
{
    public CombatActionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenPlayerInRoom_WhenGettingAvailability_ThenSomePlayersAreVisible()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid bruteId = await GetPlayerId(gameDataSeed, Roles.Brute);

        ActionDto actionDto = await GetCombatAction(bruteId);

        Assert.NotEmpty(actionDto.Prompts[CombatAction.PLAYER_PROMPT_INDEX].ValidTargets);
    }

    [Fact]
    public async Task GivenPlayerInRoom_WhenGettingCombatAction_ThenCanChooseAnAttackType()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid bruteId = await GetPlayerId(gameDataSeed, Roles.Brute);

        ActionDto actionDto = await GetCombatAction(bruteId);

        Assert.Equal(Enum.GetValues<AttackTypes>().Length,
            actionDto.Prompts[CombatAction.ATTACKTYPE_PROMPT_INDEX].ValidTargets.Count);
    }

    [Fact]
    public async Task GivenMarineStartingLoadout_WhenGettingCombatAction_ThenHeCanShootHisMusket()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid marineId = await GetPlayerId(gameDataSeed, Roles.Marine);

        ActionDto actionDto = await GetCombatAction(marineId);

        Assert.Contains(actionDto.Prompts[CombatAction.RANGED_WEAPON_PROMPT_INDEX].ValidTargets,
            x => x.Value == nameof(Weapons.Musket));
    }

    [Fact]
    public async Task GivenChaplain_WhenGettingCombatAction_ThenHeCannotAttack()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid chaplainId = await GetPlayerId(gameDataSeed, Roles.Chaplain);

        ActionDto actionDto = await GetCombatAction(chaplainId);

        Assert.False(actionDto.IsExecutable());
    }

    [Fact]
    public async Task GivenBruteAttacksTheEngineerToTheEnd_WhenFightEnds_ThenEngineerIsDead()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid bruteId = await GetPlayerId(gameDataSeed, Roles.Brute);
        Guid engineerId = await GetPlayerId(gameDataSeed, Roles.Engineer);

        await Attack(bruteId, engineerId, AttackerStances.ToTheEnd, AttackTypes.Kill);
        RefreshScope();

        Player engineer = await PlayerRepository.GetPlayer(engineerId);
        Assert.Equal(PlayerLifeStatus.Dead, engineer.LifeStatus);
    }

    [Fact]
    public async Task GivenBruteStunsTheEngineerToTheEnd_WhenFightEnds_ThenEngineerIsStunned()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid bruteId = await GetPlayerId(gameDataSeed, Roles.Brute);
        Guid engineerId = await GetPlayerId(gameDataSeed, Roles.Engineer);

        await Attack(bruteId, engineerId, AttackerStances.ToTheEnd, AttackTypes.Stun);
        RefreshScope();

        Player engineer = await PlayerRepository.GetPlayer(engineerId);
        Assert.Equal(PlayerLifeStatus.Stunned, engineer.LifeStatus);
    }

    [Fact]
    public async Task GivenBruteAttacksTheEngineer_WhenFightEnds_ThenEngineerIsToldSomethingInHisLogs()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid bruteId = await GetPlayerId(gameDataSeed, Roles.Brute);
        Guid engineerId = await GetPlayerId(gameDataSeed, Roles.Engineer);

        await Attack(bruteId, engineerId, AttackerStances.ToTheEnd, AttackTypes.Kill);
        RefreshScope();

        Player engineer = await PlayerRepository.GetPlayer(engineerId);
        Assert.NotEmpty(Context.Entry(engineer).Collection(x => x.GameLogs).Query().ToList());
    }

    [Fact]
    public async Task GivenEngineerIsDead_WhenBruteGetsCombatAction_ThenEngineerIsNoLongerATarget()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid bruteId = await GetPlayerId(gameDataSeed, Roles.Brute);
        Guid engineerId = await GetPlayerId(gameDataSeed, Roles.Engineer);
        await Attack(bruteId, engineerId, AttackerStances.ToTheEnd, AttackTypes.Kill);
        RefreshScope();

        ActionDto actionDto = await GetCombatAction(bruteId);

        Assert.DoesNotContain(actionDto.Prompts[CombatAction.PLAYER_PROMPT_INDEX].ValidTargets,
            x => x.TargetId == engineerId);
    }

    private async Task<Guid> GetPlayerId(GameDataSeed gameDataSeed, Roles role)
    {
        Player player = await Context.Players.SingleAsync(x => x.GameId == gameDataSeed.GameId && x.Role == role);

        return player.Id;
    }

    private async Task<ActionDto> GetCombatAction(Guid playerId)
    {
        GetActionsResult actions = await Mediator.Send(new GetActionsRequest
        {
            PlayerId = playerId
        });

        ActionDto actionDto = actions.Actions.First(x => x.Name == ActionNames.CombatAction);

        return actionDto;
    }

    /// <summary>
    /// Melee attack with bare hands. Acolytes get one more prompt (faked Hope): the first value is picked.
    /// </summary>
    private async Task Attack(Guid attackerId, Guid defenderId, AttackerStances stance, AttackTypes attackType)
    {
        ActionDto actionDto = await GetCombatAction(attackerId);

        List<List<ActionTarget>> targets =
        [
            [actionDto.Prompts[CombatAction.PLAYER_PROMPT_INDEX].ValidTargets.First(x => x.TargetId == defenderId)],
            [ValueOf(actionDto, CombatAction.ATTACKERSTANCE_PROMPT_INDEX, stance.ToString())],
            [ValueOf(actionDto, CombatAction.ATTACKTYPE_PROMPT_INDEX, attackType.ToString())],
            [ValueOf(actionDto, CombatAction.RANGED_WEAPON_PROMPT_INDEX, CombatAction.NO_RANGED_WEAPON)],
            [ValueOf(actionDto, CombatAction.MELEE_WEAPON_PROMPT_INDEX, nameof(Weapons.Unarmed))],
        ];

        if (actionDto.Prompts.Count > CombatAction.FAKED_HOPE_PENALTY_PROMPT_INDEX)
        {
            targets.Add([actionDto.Prompts[CombatAction.FAKED_HOPE_PENALTY_PROMPT_INDEX].ValidTargets.First()]);
        }

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.CombatAction,
            PlayerId = attackerId,
            ActionTargets = targets
        });
    }

    private static ActionTarget ValueOf(ActionDto actionDto, int promptIndex, string value)
    {
        return actionDto.Prompts[promptIndex].ValidTargets.First(x => x.Value == value);
    }
}
