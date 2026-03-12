namespace AetherFire23.ERP.Domain.Features.Actions.General.Combat;

public enum FightLoopStates
{
    Ongoing,
    ExittedBecauseOfPushHard,
    ExittedBecauseOfPlayerDeath,
    EarlyExittedBecauseOfHitAndRun,
}