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
            {
                if (!tree.haveChangeZone)
                   tree.haveChangeZone = true;
                return BTNodeState.SUCCESS;
            }

            if (tree.haveChangeZone)
            {
                tree.zone = Random.Range(0, 3);

                if (tree.zone == tree.lastZone)
                {
                    tree.zone = (tree.zone + 1) % 3;
                }
                tree.transform.position = tree.tpZones[tree.zone].position;
                tree.haveChangeZone = false;
                tree.lastZone = tree.zone;
            }

            return BTNodeState.RUNNING;
        }
    }
}
