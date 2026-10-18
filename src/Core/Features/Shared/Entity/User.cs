namespace NorthwestV2.Features.Features.Shared.Entity;

public class User : EntityBase
{
    public required string Username { get; set; } = string.Empty;
    public required string HashedPassword { get; set; }

    public Guid? LobbyId { get; set; }
    public virtual Lobby? Lobby { get; set; }

    public virtual Player? Player { get; set; }
}