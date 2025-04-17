using BehaviorTree;
using UnityEngine;

namespace AI.Zeus
{
    public class BTAction_Cooldown : BTNode
    {
        private BTZeusTree tree;

        public BTAction_Cooldown(BTZeusTree btParent)
        {
            tree = btParent;
        }

        public override BTNodeState Evaluate()
        {
            if(tree.currentPattern == null || tree.currentPatternIndex >= tree.currentPattern.cloudSpawnsWithDurations.Count)
            {
                tree.currentPatternIndex = 0;
                return BTNodeState.FAILURE;
            }

            if (Time.time >= tree.nextAttackTime)
                return BTNodeState.SUCCESS;

            return BTNodeState.RUNNING;
        }
    }
}
