using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Debug;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Features.Actions.General.Combat;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Debug;

public class DebugInstantActionApp : InstantActionBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly DebugInstantAction _debugInstantAction;

    public DebugInstantActionApp(DebugInstantAction debugInstantAction, IPlayerRepository playerRepository,
        IUnitOfWork unitOfWork) : base(
        ActionNames
            .InstantHeal)
    {
        _debugInstantAction = debugInstantAction;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        InstantActionAvailability availability = _debugInstantAction.GetAvailability(player);

        return availability;
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        player.Health += 2;

        await _unitOfWork.SaveChangesAsync();
    }
}