using NorthwestV2.Features.Features.Actions.Core.Application.Bases;
using NorthwestV2.Features.Features.Actions.Domain.Core;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.Requirements;
using NorthwestV2.Features.Features.Actions.Domain.Core.Availability.WithTargets;
using NorthwestV2.Features.Features.Actions.Domain.General.Combat;
using NorthwestV2.Features.Features.Shared.Entity;
using NorthwestV2.Features.Repositories;
using NorthwestV2.Features.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Features.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Features.Features.Actions.General.Combat;

public class ChooseDefensiveCounterApp : ActionWithTargetsBase
{
    private readonly ChooseDefensiveCounter _chooseDefensiveCounter;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;
    private readonly ChooseDefensiveCounter chooseDefensiveCounter1;

    public ChooseDefensiveCounterApp(ChooseDefensiveCounter chooseDefensiveCounter, IUnitOfWork unitOfWork,
        IPlayerRepository playerRepository, ChooseDefensiveCounter chooseDefensiveCounter1) : base(
        ActionNames
            .PickDefensiveStance)
    {
        _chooseDefensiveCounter = chooseDefensiveCounter;
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
        this.chooseDefensiveCounter1 = chooseDefensiveCounter1;
    }

    // TODO: Doing too much; move to domain layer plz 
    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        // Always only the defensive counters enum transformed into a target. 
        TargetSelectionPrompt targetSelectionPrompt = _chooseDefensiveCounter.CreatePromptOfDefensiveCounters();

        ActionWithTargetsAvailability stuff = new ActionWithTargetsAvailability
        {
            ActionName = ActionName,
            TargetSelectionPrompts = [targetSelectionPrompt],
            ActionRequirements =
            [
                new ActionRequirement()
                {
                    Description = "Allo",
                    IsFulfilled = true
                }
            ],
            DisplayName = this.ActionName
        };

        return stuff;
    }


    /*
     * Execution part
     */

    public override async Task Execute(ExecuteActionRequest request)
    {
        DefensiveCounters defensiveCounter = ExtractDefensiveCounterFromActionTargets(request.ActionTargets);

        Player player = await _playerRepository.GetPlayer(request.PlayerId);

        _chooseDefensiveCounter.Execute(player, defensiveCounter);

        await _unitOfWork.SaveChangesAsync();
    }

    private DefensiveCounters ExtractDefensiveCounterFromActionTargets(List<List<ActionTarget>> actionTargets)
    {
        if (actionTargets.Count > 1)
        {
            throw new Exception("Should not exceed 1 screen wth");
        }

        ActionTarget? target = actionTargets.FirstOrDefault()?.FirstOrDefault();

        if (target?.Value is null)
        {
            throw new Exception("Is null exception. ");
        }

        // Expects an enum.ToString()
        DefensiveCounters defensiveCounters = Enum.Parse<DefensiveCounters>(target.Value);

        return defensiveCounters;
    }
}