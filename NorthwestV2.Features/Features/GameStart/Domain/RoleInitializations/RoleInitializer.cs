using NorthwestV2.Features.Features.Shared.Entity;

namespace NorthwestV2.Features.Features.GameStart.RoleInitializations;

public abstract class RoleInitializer
{
    public abstract Player CreateAndInitializePlayer(RoleInitializationContext context);

    public Roles Role { get; }

    protected RoleInitializer(Roles role)
    {
        this.Role = role;
    }
}