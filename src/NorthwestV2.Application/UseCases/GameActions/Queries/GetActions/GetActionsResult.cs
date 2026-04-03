using AetherFire23.ERP.Domain.Features.Actions.Core;

namespace NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

public class GetActionsResult
{
    public required List<ActionDto> Actions { get; set; }

    public bool HasAction(string actionName)
    {
        bool actionIsPresent = this.Actions.Any(x => x.Name == actionName);

        return actionIsPresent;
    }

    public ActionDto GetAction(string actionName)
    {
        ActionDto action = this.Actions.First(x => x.Name == actionName);

        return action;
    }
}