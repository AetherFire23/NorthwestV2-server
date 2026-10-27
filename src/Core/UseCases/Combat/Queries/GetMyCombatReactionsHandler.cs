using Mediator;
using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Actions.General.Combat.StartCombat.Domain;
using NorthwestV2.Features.Shared.Entity;

namespace NorthwestV2.UseCases.Combat.Queries;

public class GetMyCombatReactionsHandler : IRequestHandler<GetMyCombatReactionsRequest, GetMyCombatReactionsResponse>
{
    private readonly IPlayerRepository _playerRepository;

    public GetMyCombatReactionsHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async ValueTask<GetMyCombatReactionsResponse> Handle(GetMyCombatReactionsRequest request,
        CancellationToken cancellationToken)
    {
        Player player = await _playerRepository.GetPlayerForCombat(request.PlayerId);
        List<Player> opponents = await _playerRepository.GetOtherPlayersInGame(request.PlayerId);

        List<CombatReactionDto> reactions = opponents
            .SelectMany(opponent => Enum.GetValues<AttackTypes>()
                .Select(attackType => MapReaction(player, opponent, attackType)))
            .ToList();

        return new GetMyCombatReactionsResponse { Reactions = reactions };
    }

    private static CombatReactionDto MapReaction(Player player, Player opponent, AttackTypes attackType)
    {
        CombatReaction reaction = player.GetCombatReaction(opponent.Id, attackType);

        CombatReactionDto dto = new CombatReactionDto
        {
            OpponentId = opponent.Id,
            OpponentRole = opponent.Role,
            AttackType = attackType,
            Counter = reaction.Counter,
            WeaponPolicy = reaction.WeaponPolicy,
            FakedHopePenalty = player.IsAcolyte ? reaction.FakedHopePenalty : null,
        };

        return dto;
    }
}
