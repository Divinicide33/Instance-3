using BehaviorTree;
using UnityEngine;

public class BTAction_DashChance : BTNode
{
    private HermèsBehaviorTree Tree;

    public BTAction_DashChance(HermèsBehaviorTree btParent)
    {
        Tree = btParent;
    }

    public override BTNodeState Evaluate()
    {
        switch (Tree.Action)
        {
            case HermèsBehaviorTree.HermesAction.None:
                float dashChance = Random.Range(0f, 100f);
                if (dashChance <= Tree.DashChance)
                {
                    Tree.Action = HermèsBehaviorTree.HermesAction.Dash;
                    return BTNodeState.SUCCESS;
                }
                Tree.Action = HermèsBehaviorTree.HermesAction.Jump;
                break;
            case HermèsBehaviorTree.HermesAction.Dash:
                return BTNodeState.SUCCESS;
            case HermèsBehaviorTree.HermesAction.Jump:
                return BTNodeState.FAILURE;
            default:
                break;
        }
        return BTNodeState.FAILURE;
    }
}
