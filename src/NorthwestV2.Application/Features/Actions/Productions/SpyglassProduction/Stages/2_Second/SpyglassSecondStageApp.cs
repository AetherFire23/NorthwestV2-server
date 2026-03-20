using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.Instant;
using NorthwestV2.Application.Features.Actions.Core.Bases;
using NorthwestV2.Application.UseCases.GameActions.Command.ExecuteAction;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions;

namespace NorthwestV2.Application.Features.Actions.Productions.SpyglassProduction.Stages._2_Second;

public class SpyglassSecondStageApp : InstantActionBase
{
    public SpyglassSecondStageApp(string actionName) : base(actionName)
    {
    }

    public override Task<InstantActionAvailability> GetAvailabilityResult(GetActionsRequest request)
    {
        // Is available when :
        /*
         * 2. is in forecastle
         * 3. Is in workshop
         *  3. has time points to contribute.
         */


        throw new NotImplementedException();
    }

    public override Task Execute(ExecuteActionRequest request)
    {
        /*
         * Stage.
         * Contribute 1 point
         * if Points == Threshold
         * Advantage to the next stage.
         */
        throw new NotImplementedException();
    }
}