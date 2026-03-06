using AetherFire23.ERP.Domain.Actions;
using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using JetBrains.Annotations;
using NorthwestV2.Application.UseCases.GameActions.Queries.GetActions.ReturnValue;

namespace ERP.Testing.Domain.Actions;

[TestSubject(typeof(GameActionsWithTargetsValidator))]
public class GameActionsWithTargetsValidatorTest
{
    private static ActionRequirement UNFULFILLED_ACTION_REQUIREMENT = new ActionRequirement()
    {
        Description = "Some invalid action requirement",
        IsFulfilled = false,
    };

    private static ActionRequirement FULFILLED_ACTION_REQUIREMENT = new ActionRequirement()
    {
        Description = "Fulfileld requirement ",
        IsFulfilled = true,
    };

    private static ActionWithTargetsAvailability EMPTY_TARGETS_FAILING_REQUIREMENTS = new ActionWithTargetsAvailability
    {
        ActionName = "blabla",
        ActionRequirements = [UNFULFILLED_ACTION_REQUIREMENT]
    };

    private static TargetSelectionPrompt MIN1_MAX_1_TARGET_SELECTION_PROMPT = new TargetSelectionPrompt()
    {
        Description = "MIN MAX 1 TARGET SELECTION PROMPT",
        Targets = [],
    };

    private static TargetSelectionPrompt MIN1_MAX_2_TARGET_SELECTION_PROMPT = new TargetSelectionPrompt()
    {
        Description = "MIN MAX 1 TARGET SELECTION PROMPT",
        Targets = new(),
        Min = 1,
        Max = 2
    };

    private static TargetSelectionPrompt MIN_2_MAX_2_TARGET_SELECTION_PROMPT = new TargetSelectionPrompt()
    {
        Description = "MIN MAX 1 TARGET SELECTION PROMPT",
        Targets = new(),
        Min = 2,
        Max = 2
    };

    private static ActionWithTargetsAvailability SINGLE_SCREEN_MIN_1_MAX_1_ = new ActionWithTargetsAvailability
    {
        ActionName = "blabla",
        ActionRequirements = [FULFILLED_ACTION_REQUIREMENT],
        TargetSelectionPrompts = [MIN1_MAX_1_TARGET_SELECTION_PROMPT]
    };

    private static List<List<ActionTarget>> ANY_ACTION_TARGETS = [[]];

    private static ActionTarget ANY_TARGET_WITH_ID_1 = new ActionTarget
    {
        TargetId = new Guid("6ab835b4-8b70-4e8e-97ec-937e6d012e5c"),
    };

    private static ActionTarget ANY_TARGET_WITH_ID_2 = new ActionTarget
    {
        TargetId = new Guid("7e17ca2f-9130-475e-b7bb-ef64dc05d47b"),
    };

    private static ActionTarget ANY_TARGET_WITH_ID_3 = new ActionTarget
    {
        TargetId = new Guid("31238f51-7cc5-451e-9380-a49faa7d560a"),
    };

    private static ActionTarget ANY_TARGET_WITH_ID_4 = new ActionTarget
    {
        TargetId = new Guid("c98b8006-1a5e-4495-8655-b40897758549"),
    };

    [Fact]
    public void GivenUnfulfilledActionRequirements_WhenValidated_ThenThrows()
    {
        GameActionsWithTargetsValidator gameActionsWithTargetsValidator = new GameActionsWithTargetsValidator();

        Action action = () =>
            gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow(EMPTY_TARGETS_FAILING_REQUIREMENTS,
                ANY_ACTION_TARGETS);

        Assert.Throws<Exception>(action);
    }

    [Fact]
    public void GivenSinglePrompt_WhenValidated_ThenDoesNotThrow()
    {
        GameActionsWithTargetsValidator gameActionsWithTargetsValidator = new GameActionsWithTargetsValidator();

        // gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow();
    }
}