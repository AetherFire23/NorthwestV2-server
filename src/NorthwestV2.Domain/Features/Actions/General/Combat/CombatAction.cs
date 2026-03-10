using AetherFire23.ERP.Domain.Actions.ByRoles.General.Combat;

namespace AetherFire23.ERP.Domain.Features.Actions.General.Combat;

public class CombatAction
{
    /*
     * Attack types
     *  - Kill
     *  - Stun
     *
     * Defensive Counters ( Defined in another action
     *  - Default vs stun
     *  - Override - kill counter
     *
     * Stances
     *  - Hit and run -> Disengages immediately if strength gap is too low; attacker avoids damage and likely avoids identification.
     *  - Push Hard -> Attacker continues until losing inevitable, then flees, if no kill achieved, they are identified.
     *
     */


    public List<AttackTypes> ProvideValidAttackTypes()
    {
        return Enum.GetValues<AttackTypes>().ToList();
    }
}