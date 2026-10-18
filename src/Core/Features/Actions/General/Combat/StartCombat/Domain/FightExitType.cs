namespace NorthwestV2.Features.Features.Actions.General.Combat.StartCombat.Domain;

public enum FightExitType
{
    Ongoing,
    ExittedBecauseOfPushHard,
    ExittedBecauseOfPlayerDeath,
    EarlyExittedBecauseOfHitAndRun,
}