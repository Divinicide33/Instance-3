using BehaviorTree;
using UnityEngine;

public class BTAction_DashChance : BTNode
{
    private Herm�sBehaviorTree Tree;

    public BTAction_DashChance(Herm�sBehaviorTree btParent)
    {
        Tree = btParent;
    }

    public override BTNodeState Evaluate()
    {
        switch (Tree.Action)
        {
            case Herm�sBehaviorTree.HermesAction.None:
                float dashChance = Random.Range(0f, 100f);
                if (dashChance <= Tree.DashChance)
                {
                    Tree.Action = Herm�sBehaviorTree.HermesAction.Dash;
                    return BTNodeState.SUCCESS;
                }
                Tree.Action = Herm�sBehaviorTree.HermesAction.Jump;
                break;
            case Herm�sBehaviorTree.HermesAction.Dash:
                return BTNodeState.SUCCESS;
            case Herm�sBehaviorTree.HermesAction.Jump:
                return BTNodeState.FAILURE;
            default:
                break;
        }
        return BTNodeState.FAILURE;
    }
}
