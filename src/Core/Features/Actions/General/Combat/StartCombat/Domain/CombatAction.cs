using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.Requirements;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;

/// <summary>
/// Domain side of the combat action: who may attack whom, the attacker's prompts, and applying a resolved fight to
/// the players. Resolution itself is <see cref="CombatResolver"/>. Nothing is saved here, see <see cref="CombatActionApp"/>.
/// Allegiance is ignored on purpose: two acolytes may fight each other (see docs/traitors.md).
/// </summary>
public class CombatAction
{
    public const int PLAYER_PROMPT_INDEX = 0;
    public const int ATTACKERSTANCE_PROMPT_INDEX = 1;
    public const int ATTACKTYPE_PROMPT_INDEX = 2;
    public const int RANGED_WEAPON_PROMPT_INDEX = 3;
    public const int MELEE_WEAPON_PROMPT_INDEX = 4;

    /// <summary>
    /// Only present for acolytes.
    /// </summary>
    public const int FAKED_HOPE_PENALTY_PROMPT_INDEX = 5;

    /// <summary>
    /// Value of the ranged weapon prompt for a melee attack.
    /// </summary>
    public const string NO_RANGED_WEAPON = "None";

    private readonly CombatSetupFactory _combatSetupFactory;
    private readonly CombatResolver _combatResolver;

    public CombatAction(CombatSetupFactory combatSetupFactory, CombatResolver combatResolver)
    {
        _combatSetupFactory = combatSetupFactory;
        _combatResolver = combatResolver;
    }

    public ActionWithTargetsAvailability DetermineAvailability(Player caster, List<Player> otherPlayersInSameRoom)
    {
        if (otherPlayersInSameRoom.Any(x => x.Id == caster.Id))
        {
            throw new Exception("The caster cannot target himself.");
        }

        List<Player> targets = otherPlayersInSameRoom
            .Where(x => x.LifeStatus == PlayerLifeStatus.Alive)
            .ToList();

        ActionWithTargetsAvailability availability = new ActionWithTargetsAvailability()
        {
            ActionName = ActionNames.CombatAction,
            DisplayName = ActionNames.CombatAction,
            TargetSelectionPrompts = CreatePrompts(caster, targets),
            ActionRequirements = DetermineRequirements(caster, targets),
        };

        return availability;
    }

    private static List<ActionRequirement> DetermineRequirements(Player caster, List<Player> targets)
    {
        ActionRequirement isConscious = new ActionRequirement(
            "You must be alive and conscious.",
            caster.LifeStatus == PlayerLifeStatus.Alive);

        ActionRequirement isNotChaplain = new ActionRequirement(
            "The Chaplain can never attack humans.",
            caster.Role != Roles.Chaplain);

        ActionRequirement hasTarget = ActionRequirement.CreateAnyOtherPlayerExistsInRoomRequirement(caster, targets);

        return [isConscious, isNotChaplain, hasTarget];
    }

    private static List<TargetSelectionPrompt> CreatePrompts(Player caster, List<Player> targets)
    {
        List<TargetSelectionPrompt> prompts =
        [
            TargetSelectionPrompt.FromPlayers(targets),
            TargetSelectionPrompt.FromEnum<AttackerStances>("Pick your attacker stance"),
            TargetSelectionPrompt.FromEnum<AttackTypes>("Pick your attack type"),
            CreateRangedWeaponPrompt(caster),
            CreateMeleeWeaponPrompt(caster),
        ];

        if (caster.IsAcolyte)
        {
            prompts.Add(TargetSelectionPrompt.FromEnum<FakedHopePenalties>("Pick the Hope penalty you fake"));
        }

        return prompts;
    }

    /// <summary>
    /// Ignored for a stun attack, which is always unarmed.
    /// </summary>
    private static TargetSelectionPrompt CreateRangedWeaponPrompt(Player caster)
    {
        IEnumerable<string> values = WeaponCatalog
            .OwnedWeapons(caster, WeaponRanges.Ranged, includeConcealed: true)
            .Select(x => x.ToString());

        return CreateValuePrompt("Pick a ranged weapon (none for a melee attack)", [NO_RANGED_WEAPON, ..values]);
    }

    private static TargetSelectionPrompt CreateMeleeWeaponPrompt(Player caster)
    {
        IEnumerable<string> values = WeaponCatalog
            .OwnedWeapons(caster, WeaponRanges.Melee, includeConcealed: true)
            .Select(x => x.ToString());

        return CreateValuePrompt("Pick a melee weapon", [Weapons.Unarmed.ToString(), ..values]);
    }

    private static TargetSelectionPrompt CreateValuePrompt(string description, List<string> values)
    {
        TargetSelectionPrompt prompt = new TargetSelectionPrompt
        {
            Description = description,
            ValidTargets = values
                .Select(x => new ActionTarget { Name = x, Value = x, TargetId = Guid.Empty })
                .ToList(),
        };

        return prompt;
    }

    /// <summary>
    /// Resolves the fight and writes Health and life status onto both players.
    /// </summary>
    public FightResult MakeTwoPlayerFightTogether(Player attacker, Player defender, AttackerChoices choices)
    {
        bool isOutside = RoomExposure.IsOutside(attacker.Room.RoomEnum);

        CombatSetup setup = _combatSetupFactory.Create(attacker, defender, choices, isOutside);
        FightResult fightResult = _combatResolver.Resolve(setup);

        ApplyEndFightConditions(fightResult);

        return fightResult;
    }

    /// <summary>
    /// Only Health and life status outlive the fight.
    /// </summary>
    public void ApplyEndFightConditions(FightResult fightResult)
    {
        ApplyToPlayer(fightResult.Attacker, fightResult.AttackerStatus);
        ApplyToPlayer(fightResult.Defender, fightResult.DefenderStatus);
    }

    private static void ApplyToPlayer(Combatant combatant, PlayerLifeStatus status)
    {
        combatant.Player.Health = Math.Max(0, combatant.Health);
        combatant.Player.LifeStatus = status;
    }

    public Guid DetermineDefenderPlayer(List<List<ActionTarget>> requestActionTargets)
    {
        Guid defenderPlayerId = requestActionTargets[PLAYER_PROMPT_INDEX].First().TargetId ??
                                throw new Exception("target it cannot be null.");

        return defenderPlayerId;
    }

    public AttackerChoices DetermineAttackerChoices(List<List<ActionTarget>> requestActionTargets, Player attacker)
    {
        string rangedValue = ReadValue(requestActionTargets, RANGED_WEAPON_PROMPT_INDEX);

        AttackerChoices choices = new AttackerChoices
        {
            Stance = Enum.Parse<AttackerStances>(ReadValue(requestActionTargets, ATTACKERSTANCE_PROMPT_INDEX)),
            AttackType = Enum.Parse<AttackTypes>(ReadValue(requestActionTargets, ATTACKTYPE_PROMPT_INDEX)),
            RangedWeapon = rangedValue == NO_RANGED_WEAPON ? null : Enum.Parse<Weapons>(rangedValue),
            MeleeWeapon = Enum.Parse<Weapons>(ReadValue(requestActionTargets, MELEE_WEAPON_PROMPT_INDEX)),
            FakedHopePenalty = attacker.IsAcolyte
                ? Enum.Parse<FakedHopePenalties>(ReadValue(requestActionTargets, FAKED_HOPE_PENALTY_PROMPT_INDEX))
                : FakedHopePenalties.MatchTrackedHope,
        };

        return choices;
    }

    private static string ReadValue(List<List<ActionTarget>> requestActionTargets, int promptIndex)
    {
        return requestActionTargets[promptIndex].First().Value ??
               throw new Exception($"Prompt {promptIndex} of the combat action needs a value.");
    }
}
