using AetherFire23.ERP.Domain.Actions;
using AetherFire23.ERP.Domain.Actions.AvailabilityStuff;
using AetherFire23.ERP.Domain.Features.Actions.Core;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability;
using AetherFire23.ERP.Domain.Features.Actions.Core.Availability.WithTargets;
using JetBrains.Annotations;

namespace ERP.Testing.Domain.Actions;

[TestSubject(typeof(GameActionsWithTargetsValidator))]
public class GameActionsWithTargetsValidatorTest
{
    private static ActionTarget ANY_TARGET_WITH_ID_1 = new ActionTarget
    {
        TargetId = new Guid("6ab835b4-8b70-4e8e-97ec-937e6d012e5c"),
    };


    // == value prompts ==

    private static TargetSelectionPrompt VALUE_PROMPT = new TargetSelectionPrompt()
    {
        Description = nameof(VALUE_PROMPT),
        ValidTargets = [new ActionTarget() { Value = "1" }]
    };

    private static ActionWithTargetsAvailability AVAILABILY_ONE_PROMPT_VALUES_MIN_1_MAX_1 =
        new ActionWithTargetsAvailability
        {
            ActionName = "blabla",
            ActionRequirements = [new ActionRequirement() { Description = "wrks", IsFulfilled = true }],
            TargetSelectionPrompts = [VALUE_PROMPT]
        };


    private static ActionTarget SAME_TARGET_WITH_VALUE_1 = new ActionTarget
    {
        Value = "1"
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

    private static ActionTarget ANY_RANDOM_TARGET = new ActionTarget
    {
        TargetId = new Guid("7b2c9c78-08f1-4df9-be69-358c3da5f6b8"),
    };

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

    private static TargetSelectionPrompt MIN1_MAX_1_TWO_TARGETS_PROMPT_1 = new TargetSelectionPrompt()
    {
        Description = nameof(MIN1_MAX_1_TWO_TARGETS_PROMPT_1),
        ValidTargets = [ANY_TARGET_WITH_ID_1, ANY_TARGET_WITH_ID_2],
    };

    private static TargetSelectionPrompt MIN1_MAX_1_PROMPT__2 = new TargetSelectionPrompt()
    {
        Description = nameof(MIN1_MAX_1_PROMPT__2),
        ValidTargets = [ANY_TARGET_WITH_ID_3, ANY_TARGET_WITH_ID_4],
    };

    private static TargetSelectionPrompt MIN1_MAX_2_PROMPT_2 = new TargetSelectionPrompt()
    {
        Description = "MIN MAX 1 TARGET SELECTION PROMPT",
        ValidTargets = [ANY_TARGET_WITH_ID_2],
        Min = 1,
        Max = 1
    };

    private static TargetSelectionPrompt MIN_2_MAX_2_TARGET_SELECTION_PROMPT = new TargetSelectionPrompt()
    {
        Description = "MIN MAX 1 TARGET SELECTION PROMPT",
        ValidTargets = new(),
        Min = 2,
        Max = 2
    };

    private static ActionWithTargetsAvailability AVAILABILITY_SINGLE_PROMPT_MIN_1_MAX_1_ =
        new ActionWithTargetsAvailability
        {
            ActionName = "blabla",
            ActionRequirements = [FULFILLED_ACTION_REQUIREMENT],
            TargetSelectionPrompts = [MIN1_MAX_1_TWO_TARGETS_PROMPT_1]
        };

    private static ActionWithTargetsAvailability TWO_PROMPTS_MIN_1_MAX_1_ = new ActionWithTargetsAvailability
    {
        ActionName = "blabla",
        ActionRequirements = [FULFILLED_ACTION_REQUIREMENT],
        TargetSelectionPrompts = [MIN1_MAX_1_TWO_TARGETS_PROMPT_1, MIN1_MAX_1_PROMPT__2]
    };


    private static List<List<ActionTarget>> ANY_ACTION_TARGETS = [[]];
    private static List<List<ActionTarget>> ONE_PROMPT_ONE_SELECTION_ACTION_TARGET = [[ANY_TARGET_WITH_ID_1]];
    private static List<List<ActionTarget>> ONE_PROMPT_ONE_SELECTION_RANDOM_ACTION_TARGET = [[ANY_RANDOM_TARGET]];

    private static List<List<ActionTarget>> ONE_PROMPT_MANY_SELECTIONS_ACTION_TARGET =
        [[ANY_TARGET_WITH_ID_1, ANY_TARGET_WITH_ID_2]];

    private static List<List<ActionTarget>> ONE_PROMPT_TOO_MANY_SELECTIONS_ACTION_TARGET =
        [[ANY_TARGET_WITH_ID_1, ANY_TARGET_WITH_ID_2, ANY_TARGET_WITH_ID_3, ANY_TARGET_WITH_ID_4]];

    private static List<List<ActionTarget>> TWO_PROMPT_ONE_SELECTION_ACTION_TARGET =
        [[ANY_TARGET_WITH_ID_1], [ANY_TARGET_WITH_ID_3]];


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

        gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow(AVAILABILITY_SINGLE_PROMPT_MIN_1_MAX_1_,
            ONE_PROMPT_ONE_SELECTION_ACTION_TARGET);
    }

    [Fact]
    public void GivenTwoPrompts_WhenValidated_ThenDoesNotThrow()
    {
        GameActionsWithTargetsValidator gameActionsWithTargetsValidator = new GameActionsWithTargetsValidator();

        gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow(TWO_PROMPTS_MIN_1_MAX_1_,
            TWO_PROMPT_ONE_SELECTION_ACTION_TARGET);
    }

    [Fact]
    public void GivenOnePromptWithTooFewSelections_WhenValidated_ThenThrows()
    {
        GameActionsWithTargetsValidator gameActionsWithTargetsValidator = new GameActionsWithTargetsValidator();

        Action action = () =>
            gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow(AVAILABILITY_SINGLE_PROMPT_MIN_1_MAX_1_,
                ONE_PROMPT_MANY_SELECTIONS_ACTION_TARGET);

        Assert.Throws<Exception>(action);
    }

    [Fact]
    public void GivenOnePromptWithTooManySelections_WhenValidated_ThenThrows()
    {
        GameActionsWithTargetsValidator gameActionsWithTargetsValidator = new GameActionsWithTargetsValidator();

        Action action = () =>
            gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow(AVAILABILITY_SINGLE_PROMPT_MIN_1_MAX_1_,
                ONE_PROMPT_TOO_MANY_SELECTIONS_ACTION_TARGET);

        Assert.Throws<Exception>(action);
    }

    [Fact]
    public void GivenInvalidTargetProvided_WhenValidated_ThenThrows()
    {
        GameActionsWithTargetsValidator gameActionsWithTargetsValidator = new GameActionsWithTargetsValidator();

        Action action = () =>
            gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow(AVAILABILITY_SINGLE_PROMPT_MIN_1_MAX_1_,
                ONE_PROMPT_ONE_SELECTION_RANDOM_ACTION_TARGET);

        Assert.Throws<Exception>(action);
    }

    // TODO: test min maxxess
    [Fact]
    public void GivenPromptWithLargeMinMaxBounds_WhenValidated_ThenDoesNotThrow()
    {
        GameActionsWithTargetsValidator gameActionsWithTargetsValidator = new GameActionsWithTargetsValidator();
        var prompt = new TargetSelectionPrompt()
        {
            Description = "MIN2 MAX 2 TARGET SELECTION PROMPT",
            ValidTargets = [ANY_TARGET_WITH_ID_1,ANY_TARGET_WITH_ID_2],
            Min = 2,
            Max = 2
        };
        ActionWithTargetsAvailability availability = new ActionWithTargetsAvailability
        {
            ActionName = "blabla",
            ActionRequirements = [FULFILLED_ACTION_REQUIREMENT],
            TargetSelectionPrompts = [prompt]
        };
        List<List<ActionTarget>> actionsTargets = [[ANY_TARGET_WITH_ID_1, ANY_TARGET_WITH_ID_2]];

    
            gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow(availability, actionsTargets);

    }


    // === VALUE SELECTIONS ==

    [Fact]
    public void GivenSinglePromptWithValueTargets_WhenValidated_ThenDoesNotThrow()
    {
        GameActionsWithTargetsValidator gameActionsWithTargetsValidator = new GameActionsWithTargetsValidator();

        gameActionsWithTargetsValidator.EnsureIsValidActionExecutionOrThrow(AVAILABILY_ONE_PROMPT_VALUES_MIN_1_MAX_1,
            [[SAME_TARGET_WITH_VALUE_1]]);
    }
}