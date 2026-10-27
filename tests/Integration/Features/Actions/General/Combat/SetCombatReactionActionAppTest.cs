using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Actions.General.Combat.SetCombatReaction;
using NorthwestV2.Features.Actions.General.Combat.SetVigilance;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.Combat.Queries;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;
using NorthwestV2.UseCases.MyPlayer.Queries;
using Xunit.Abstractions;

namespace NorthwestV2.Integration.Features.Actions.General.Combat;

/// <summary>
/// Combat reactions and vigilance: the defender's settings, made in advance.
/// </summary>
[TestSubject(typeof(SetCombatReactionActionApp))]
public class SetCombatReactionActionAppTest : NorthwestTestBase
{
    public SetCombatReactionActionAppTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GivenNewGame_WhenReadingMyCombatReactions_ThenThereIsOnePerOpponentAndAttackType()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid engineerId = await GetPlayerId(gameDataSeed, Roles.Engineer);

        GetMyCombatReactionsResponse response =
            await Mediator.Send(new GetMyCombatReactionsRequest { PlayerId = engineerId });

        Assert.Equal((gameDataSeed.PlayerIds.Count - 1) * Enum.GetValues<AttackTypes>().Length,
            response.Reactions.Count);
    }

    [Fact]
    public async Task GivenStunCounterSet_WhenReadingMyCombatReactions_ThenItIsReturned()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid engineerId = await GetPlayerId(gameDataSeed, Roles.Engineer);
        Guid bruteId = await GetPlayerId(gameDataSeed, Roles.Brute);

        await SetStunCounterAgainstKill(engineerId, bruteId);
        RefreshScope();

        GetMyCombatReactionsResponse response =
            await Mediator.Send(new GetMyCombatReactionsRequest { PlayerId = engineerId });
        Assert.Equal(AttackTypes.Stun, response.Reactions
            .Single(x => x.OpponentId == bruteId && x.AttackType == AttackTypes.Kill).Counter);
    }

    [Fact]
    public async Task GivenStunCounterSet_WhenTheBruteLosesAKillAttackToTheEnd_ThenTheBruteIsOnlyStunned()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid engineerId = await GetPlayerId(gameDataSeed, Roles.Engineer);
        Guid bruteId = await GetPlayerId(gameDataSeed, Roles.Brute);
        Player brute = await PlayerRepository.GetPlayer(bruteId);
        brute.Health = 5;
        await Context.SaveChangesAsync();
        await SetStunCounterAgainstKill(engineerId, bruteId);
        RefreshScope();

        await AttackBareHanded(bruteId, engineerId);
        RefreshScope();

        Player bruteAfterFight = await PlayerRepository.GetPlayer(bruteId);
        Assert.Equal(PlayerLifeStatus.Stunned, bruteAfterFight.LifeStatus);
    }

    [Fact]
    public async Task GivenVigilanceTurnedOn_WhenReadingMyPlayer_ThenIAmVigilant()
    {
        GameDataSeed gameDataSeed = await ShareSeeds.ArrangeUntilGameCreation(Mediator, Context);
        Guid engineerId = await GetPlayerId(gameDataSeed, Roles.Engineer);

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SetVigilance,
            PlayerId = engineerId,
            ActionTargets = [[await FindTarget(engineerId, ActionNames.SetVigilance, 0, nameof(VigilanceSettings.Vigilant))]],
        });
        RefreshScope();

        GetMyPlayerResponse response = await Mediator.Send(new GetMyPlayerRequest { PlayerId = engineerId });
        Assert.True(response.IsVigilant);
    }

    private async Task SetStunCounterAgainstKill(Guid playerId, Guid opponentId)
    {
        ActionDto actionDto = await GetAction(playerId, ActionNames.SetCombatReaction);

        List<List<ActionTarget>> targets =
        [
            [actionDto.Prompts[SetCombatReactionAction.OPPONENT_PROMPT_INDEX].ValidTargets.First(x => x.TargetId == opponentId)],
            [ValueOf(actionDto, SetCombatReactionAction.ATTACK_TYPE_PROMPT_INDEX, nameof(AttackTypes.Kill))],
            [ValueOf(actionDto, SetCombatReactionAction.COUNTER_PROMPT_INDEX, nameof(AttackTypes.Stun))],
            [ValueOf(actionDto, SetCombatReactionAction.WEAPON_POLICY_PROMPT_INDEX, nameof(WeaponPolicies.StrongestOwned))],
        ];

        if (actionDto.Prompts.Count > SetCombatReactionAction.FAKED_HOPE_PENALTY_PROMPT_INDEX)
        {
            targets.Add([ValueOf(actionDto, SetCombatReactionAction.FAKED_HOPE_PENALTY_PROMPT_INDEX,
                nameof(FakedHopePenalties.MatchTrackedHope))]);
        }

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.SetCombatReaction,
            PlayerId = playerId,
            ActionTargets = targets,
        });
    }

    private async Task AttackBareHanded(Guid attackerId, Guid defenderId)
    {
        ActionDto actionDto = await GetAction(attackerId, ActionNames.CombatAction);

        List<List<ActionTarget>> targets =
        [
            [actionDto.Prompts[CombatAction.PLAYER_PROMPT_INDEX].ValidTargets.First(x => x.TargetId == defenderId)],
            [ValueOf(actionDto, CombatAction.ATTACKERSTANCE_PROMPT_INDEX, nameof(AttackerStances.ToTheEnd))],
            [ValueOf(actionDto, CombatAction.ATTACKTYPE_PROMPT_INDEX, nameof(AttackTypes.Kill))],
            [ValueOf(actionDto, CombatAction.RANGED_WEAPON_PROMPT_INDEX, CombatAction.NO_RANGED_WEAPON)],
            [ValueOf(actionDto, CombatAction.MELEE_WEAPON_PROMPT_INDEX, nameof(Weapons.Unarmed))],
        ];

        if (actionDto.Prompts.Count > CombatAction.FAKED_HOPE_PENALTY_PROMPT_INDEX)
        {
            targets.Add([ValueOf(actionDto, CombatAction.FAKED_HOPE_PENALTY_PROMPT_INDEX,
                nameof(FakedHopePenalties.MatchTrackedHope))]);
        }

        await Mediator.Send(new ExecuteActionRequest
        {
            ActionName = ActionNames.CombatAction,
            PlayerId = attackerId,
            ActionTargets = targets,
        });
    }

    private async Task<Guid> GetPlayerId(GameDataSeed gameDataSeed, Roles role)
    {
        Player player = await Context.Players.SingleAsync(x => x.GameId == gameDataSeed.GameId && x.Role == role);

        return player.Id;
    }

    private async Task<ActionDto> GetAction(Guid playerId, string actionName)
    {
        GetActionsResult actions = await Mediator.Send(new GetActionsRequest { PlayerId = playerId });

        return actions.Actions.First(x => x.Name == actionName);
    }

    private async Task<ActionTarget> FindTarget(Guid playerId, string actionName, int promptIndex, string value)
    {
        ActionDto actionDto = await GetAction(playerId, actionName);

        return ValueOf(actionDto, promptIndex, value);
    }

    private static ActionTarget ValueOf(ActionDto actionDto, int promptIndex, string value)
    {
        return actionDto.Prompts[promptIndex].ValidTargets.First(x => x.Value == value);
    }
}
