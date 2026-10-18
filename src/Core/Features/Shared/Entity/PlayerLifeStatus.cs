namespace NorthwestV2.Features.Features.Shared.Entity;

public enum PlayerLifeStatus
{
    Alive,

    /// <summary>
    /// Incapacitated by a stun attack at 0 Health. Not dead.
    /// </summary>
    Stunned,

    Dead,
}
