namespace NorthwestV2.Features.Features.GameStart.Domain.RoleInitializations;

/// <summary>
/// Starting toughness values for different roles. Toughness acts as a damage buffer - damage is
/// subtracted from toughness before health is lost.
/// </summary>
public class ToughnessInitializationConstants
{
    public const int WEAK = 8;
    
    public const int NORMAL = 12;
    
    public const int TOUGH = 15;
}
