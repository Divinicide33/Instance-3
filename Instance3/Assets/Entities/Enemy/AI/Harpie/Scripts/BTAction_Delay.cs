using BehaviorTree;
using UnityEngine;

namespace AI.Harpie
{
    public class BTAction_Delay : BTNode
    {
        private BTHapieTree tree;

        private float delayTimer;

        public BTAction_Delay(BTHapieTree btParent)
        {
            tree = btParent;
            delayTimer = btParent.chargeDelay;
        }

        public override BTNodeState Evaluate()
        {
            if (tree.charged)
            {
                return BTNodeState.SUCCESS;
            }

            delayTimer -= Time.deltaTime;

            if (delayTimer > 0)
            {
                return BTNodeState.RUNNING;
            }
            delayTimer = tree.chargeDelay;
            tree.charged = true;
            tree.rb.simulated = true;
            return BTNodeState.SUCCESS;
        }
    }
}
