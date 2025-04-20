using BehaviorTree;
using UnityEngine;

namespace AI.Harpie
{
    public class BTAction_Cooldown : BTNode
    {
        private BTHapieTree tree;

        public BTAction_Cooldown(BTHapieTree btParent)
        {
            tree = btParent;
        }

        public override BTNodeState Evaluate()
        {
            if (Time.time - tree.lastDiveTime >= tree.coolDownTime)
            {
                return BTNodeState.SUCCESS;
            }

            return BTNodeState.FAILURE;
        }
    }
}
