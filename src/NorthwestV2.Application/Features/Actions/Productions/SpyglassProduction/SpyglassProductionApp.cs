using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Productions.SpyglassProduction;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction;

public class SpyglassProductionApp : InstantActionBase
{
    private readonly SpyglassProductionAction _spyglassProductionAction;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    // TODO: production repository 

    public SpyglassProductionApp(SpyglassProductionAction spyglassProductionAction, IUnitOfWork unitOfWork,
        IPlayerRepository playerRepository) : base(
        ActionNames
            .SpyglassProduction)
    {
        _spyglassProductionAction = spyglassProductionAction;
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
    }

    public async override Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayerWithRoomAndInventory(request.PlayerId);


        // var availability = _spyglassProductionAction.DetermineAvailability(player,);

        // tdd that shit
        // given scrap in room and player can start productionc x

        return new InstantActionAvailability()
        {
            ActionName = "sd",
            DisplayName = "sds",
            ActionRequirements = []
        };
    }

    public async override Task Execute(ExecuteActionRequest request)
    {
        throw new NotImplementedException();
    }
}