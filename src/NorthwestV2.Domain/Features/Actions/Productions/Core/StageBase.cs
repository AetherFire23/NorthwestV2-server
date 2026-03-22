using AetherFire23.ERP.Domain.Entity;

namespace AetherFire23.ERP.Domain.Features.Actions.Productions.Core;

public abstract class StageBase
{
    public string StageName { get; set; }
    public int Contributions { get; set; } = 0;
    public int End { get; set; }

    public StageBase(int end, string stageName)
    {
        End = end;
        StageName = stageName;
    }

    public bool IsStageEnded => Contributions >= End;

    public StageBase GetNextStageIfStageEnded()
    {
        if (!IsStageEnded)
        {
            throw new Exception("Can only advance to the next stage if the current stage is over.");
        }

        StageBase nextStage = GetNextStage();

        if (nextStage is null)
        {
            throw new Exception("This stage doesn't have a next stage");
        }

        return nextStage;
    }

    public void Contribute(Player player)
    {
        if (player.ActionPoints - 1 == -1)
        {
            throw new Exception("Player does not have enough points to contribute.");
        }

        player.ActionPoints--;
        this.Contributions++;
    }

    protected virtual StageBase? GetNextStage()
    {
        return null;
    }
}