using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Entity;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using AetherFire23.ERP.Domain.Features.Actions.General.Combat;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.Repositories;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.General.Combat;

public class ChooseDefensiveCounterApp : ActionWithTargetsBase
{
    private readonly ChooseDefensiveCounter _chooseDefensiveCounter;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPlayerRepository _playerRepository;

    public ChooseDefensiveCounterApp(ChooseDefensiveCounter chooseDefensiveCounter, IUnitOfWork unitOfWork,
        IPlayerRepository playerRepository) : base(
        ActionNames
            .PickDefensiveStance)
    {
        _chooseDefensiveCounter = chooseDefensiveCounter;
        _unitOfWork = unitOfWork;
        _playerRepository = playerRepository;
    }

    public override async Task<ActionWithTargetsAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        TargetSelectionPrompt targetSelectionPrompt = CreatePromptOfDefensiveCounters();

        ActionWithTargetsAvailability stuff = new ActionWithTargetsAvailability
        {
            ActionName = ActionName,
            TargetSelectionPrompts = [targetSelectionPrompt]
        };

        return stuff;
    }

    private TargetSelectionPrompt CreatePromptOfDefensiveCounters()
    {
        List<ActionTarget> actionTargets = Enum.GetValues<DefensiveCounters>()
            .Select(x =>
                new ActionTarget()
                {
                    Value = x.ToString(),
                    Name = x.ToString(),
                    TargetId = Guid.Empty
                }).ToList();

        return new TargetSelectionPrompt()
        {
            Description = "Choose a defensive target stance.",
            ValidTargets = actionTargets
        };
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
        if (actionTargets.Count > 0)
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