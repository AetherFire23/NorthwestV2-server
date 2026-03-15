using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using Mediator;
using NorthwestV2.Application.Features.Actions.Core;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;


/// <summary>
/// Handles the execution of a game action requested by a player.
/// 
/// <list type="bullet">
///   <item>
///     <description>Resolves the action definition from its name.</description>
///   </item>
///   <item>
///     <description>Retrieves the action's availability for the requesting player.</description>
///   </item>
///   <item>
///     <description>Validates that the action can be executed according to its business rules
///     (requirements, target selection constraints, etc.).</description>
///   </item>
///   <item>
///     <description>Executes the action if all validations succeed.</description>
///   </item>
/// </list>
/// 
/// <para>
/// The handler supports both <see cref="InstantActionBase"/> (actions without target selection)
/// and <see cref="ActionWithTargetsBase"/> (actions requiring one or more target prompts).
/// Availability and validation logic are delegated to domain services and domain objects
/// </para>
/// 
/// <para>
/// If the action is no longer valid or its target selection violates domain constraints,
/// an exception is thrown and execution is aborted.
/// </para>
/// /// </summary>
public class 
    ExecuteActionHandler : IRequestHandler<ExecuteActionRequest>
{
    private readonly ActionServices _actionServices;

    private readonly GameActionsWithTargetsValidator _gameActionsWithTargetsValidator;

    public ExecuteActionHandler(ActionServices actionServices,
        GameActionsWithTargetsValidator gameActionsWithTargetsValidator)
    {
        _actionServices = actionServices;
        _gameActionsWithTargetsValidator = gameActionsWithTargetsValidator;
    }

    public async ValueTask<Unit> Handle(ExecuteActionRequest request, CancellationToken cancellationToken)
    {
        ActionBase action = await _actionServices.GetActionFromName(request.ActionName);
        
        switch (action)
        {
            case ActionWithTargetsBase actionWithTargets:
            {
                ActionWithTargetsAvailability availability = await
                    actionWithTargets.GetAvailabilityResult(new GetActionsRequest() { PlayerId = request.PlayerId });

                _gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow(availability, request.ActionTargets);
                break;
            }
            case InstantActionBase instantAction:
            {
                InstantActionAvailability availabilityResult = await instantAction.GetAvailabilityResult(new GetActionsRequest(){PlayerId = request.PlayerId});
            
                if (!availabilityResult.CanExecute)
                {
                    throw new Exception($"requested action not available anymore. {instantAction}");
                }

                break;
            }
        }

        await action.Execute(request);

        return Unit.Value;
    }
}