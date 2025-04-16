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
            tree.currentPatternIndex = 0;
            //tree.isPatternRunning = false;
            tree.nextAttackTime = Time.time + Random.Range(tree.minCooldown, tree.maxCooldown);

            Debug.Log("Chosen pattern: " + tree.currentPattern.patterns[tree.currentPatternIndex].name + ", Duration: " + tree.currentPattern.patterns[tree.currentPatternIndex].duration);

            return BTNodeState.SUCCESS;
        }

    }
}
