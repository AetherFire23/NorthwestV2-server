namespace AetherFire23.ERP.Domain.Entity;

public class User : EntityBase
{
    public required string Username { get; set; } = string.Empty;
    public required string HashedPassword { get; set; }

    public Guid? LobbyId { get; set; }
    public Lobby? Lobby { get; set; }

    public Player? Player { get; set; }
}