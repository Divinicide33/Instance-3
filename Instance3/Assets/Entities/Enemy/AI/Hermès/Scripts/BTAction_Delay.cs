using BehaviorTree;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_Delay : BTNode
    {
        private BTHermesTree tree;

        private float delayTimer;

        public BTAction_Delay(BTHermesTree btParent)
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
            return BTNodeState.SUCCESS;
        }
    }
}
