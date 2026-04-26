using NorthwestV2.Features.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Core.Domain;
using NorthwestV2.Features.Features.Actions.Core.Domain.Availability.Instant;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.ActDeDebg.Instant;

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
        player.Logs.Add(new GameLog()
        {
            Message = "You just healed someone !!!!",
        });
        await _unitOfWork.SaveChangesAsync();
    }
}