using NorthwestV2.ApplicationsStuff.Repositories;
using NorthwestV2.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Actions.Core.Domain;
using NorthwestV2.Features.Actions.Core.Domain.Availability.WithTargets;
using NorthwestV2.Features.Shared.Entity;
using NorthwestV2.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Actions.General.Combat.SetVigilance;

public class SetVigilanceActionApp : ActionWithTargetsBase
{
    private readonly SetVigilanceAction _setVigilanceAction;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetVigilanceActionApp(SetVigilanceAction setVigilanceAction, IPlayerRepository playerRepository,
        IUnitOfWork unitOfWork) : base(ActionNames.SetVigilance)
    {
        _setVigilanceAction = setVigilanceAction;
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public override Task<ActionWithTargetsAvailability?> GetAvailabilityResult(GetActionsRequest request)
    {
        return Task.FromResult<ActionWithTargetsAvailability?>(_setVigilanceAction.DetermineAvailability());
    }

    public override async Task Execute(ExecuteActionRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        _setVigilanceAction.Execute(player, request.ActionTargets);

        await _unitOfWork.SaveChangesAsync();
    }
}
