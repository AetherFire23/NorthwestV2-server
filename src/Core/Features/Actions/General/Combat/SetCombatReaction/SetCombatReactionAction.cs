using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.SetCombatReaction;

/// <summary>
/// Sets, in advance, how the player fights back when one opponent attacks him with one attack type
/// (docs/combat.md, "Combat reactions"). Acolytes get an extra prompt for the Hope penalty they fake.
/// </summary>
public class SetCombatReactionAction
{
    public const int OPPONENT_PROMPT_INDEX = 0;
    public const int ATTACK_TYPE_PROMPT_INDEX = 1;
    public const int COUNTER_PROMPT_INDEX = 2;
    public const int WEAPON_POLICY_PROMPT_INDEX = 3;

    /// <summary>
    /// Only present for acolytes.
    /// </summary>
    public const int FAKED_HOPE_PENALTY_PROMPT_INDEX = 4;

    public ActionWithTargetsAvailability DetermineAvailability(Player player, List<Player> otherPlayersInGame)
    {
        List<TargetSelectionPrompt> prompts =
        [
            TargetSelectionPrompt.FromPlayers(otherPlayersInGame),
            TargetSelectionPrompt.FromEnum<AttackTypes>("When attacked with"),
            TargetSelectionPrompt.FromEnum<AttackTypes>("Counter with"),
            TargetSelectionPrompt.FromEnum<WeaponPolicies>("Weapons to use for a kill counter"),
        ];

        if (player.IsAcolyte)
        {
            prompts.Add(TargetSelectionPrompt.FromEnum<FakedHopePenalties>("Hope penalty to fake"));
        }

        ActionWithTargetsAvailability availability = new()
        {
            ActionName = ActionNames.SetCombatReaction,
            DisplayName = "Set a combat reaction",
            TargetSelectionPrompts = prompts,
        };

        return availability;
    }

    /// <summary>
    /// Loyals keep the default faked penalty: it is never read for them.
    /// </summary>
    public void Execute(Player player, List<List<ActionTarget>> actionTargets)
    {
        Guid opponentId = actionTargets[OPPONENT_PROMPT_INDEX].First().TargetId
                          ?? throw new Exception("An opponent must be selected.");
        AttackTypes attackType = Enum.Parse<AttackTypes>(ReadValue(actionTargets, ATTACK_TYPE_PROMPT_INDEX));
        AttackTypes counter = Enum.Parse<AttackTypes>(ReadValue(actionTargets, COUNTER_PROMPT_INDEX));
        WeaponPolicies weaponPolicy = Enum.Parse<WeaponPolicies>(ReadValue(actionTargets, WEAPON_POLICY_PROMPT_INDEX));

        FakedHopePenalties fakedHopePenalty = player.IsAcolyte
            ? Enum.Parse<FakedHopePenalties>(ReadValue(actionTargets, FAKED_HOPE_PENALTY_PROMPT_INDEX))
            : CombatReaction.CreateDefault(player, opponentId, attackType).FakedHopePenalty;

        player.SetCombatReaction(opponentId, attackType, counter, weaponPolicy, fakedHopePenalty);
    }

    private static string ReadValue(List<List<ActionTarget>> actionTargets, int promptIndex)
    {
        return actionTargets[promptIndex].First().Value
               ?? throw new Exception($"Prompt {promptIndex} of the combat reaction needs a value.");
    }
}
