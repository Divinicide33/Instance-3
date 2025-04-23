using BehaviorTree;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_DashChance : BTNode
    {
        private BTHermesTree tree;

        public BTAction_DashChance(BTHermesTree btParent)
        {
            tree = btParent;
        }

        public override BTNodeState Evaluate()
        {
            switch (tree.action)
            {
                case Action.None:
                    float dashChance = Random.Range(0f, 100f);
                    
                    if (dashChance <= tree.dashChance)
                    {
                        tree.action = Action.Dash;
                        return BTNodeState.SUCCESS;
                    }
                    tree.action = Action.Jump;
                    break;

                case Action.Dash:
                    return BTNodeState.SUCCESS;
                
                case Action.Jump:
                    return BTNodeState.FAILURE;
                
                default:
                    break;
            }
            return BTNodeState.FAILURE;
        }
    }
}
