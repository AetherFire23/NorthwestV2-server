using AetherFire23.ERP.Domain.Entity;

namespace NorthwestV2.Application.Features.Actions.General.Combat;

public interface IPlayerRepository
{
    public List<Player> GetPlayersInSameRoom(Player player);
}