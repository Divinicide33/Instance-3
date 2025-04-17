using BehaviorTree;
using UnityEngine;

namespace AI.Zeus
{
    public class BTAction_ChoosePattern : BTNode
    {
        private BTZeusTree tree;

        public BTAction_ChoosePattern(BTZeusTree bt)
        {
            tree = bt;
        }

        public override BTNodeState Evaluate()
        {
            tree.currentPattern = tree.attackPatterns[Random.Range(0, tree.attackPatterns.Count)];
            tree.nextAttackTime = Time.time + Random.Range(tree.minCooldown, tree.maxCooldown);

            Debug.Log("Chosen pattern: " + tree.currentPattern.name + ", Duration: " + tree.currentPattern.cloudSpawnsWithDurations[tree.currentPattern.cloudSpawnsWithDurations.Count - 1].duration);

            return BTNodeState.SUCCESS;
        }
    }
}
