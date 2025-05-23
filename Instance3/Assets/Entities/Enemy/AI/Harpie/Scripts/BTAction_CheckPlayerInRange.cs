using BehaviorTree;
using UnityEngine;

namespace AI.Harpie
{
    public class BTAction_CheckPlayerInRange : BTNode
    {
        private BTHapieTree tree;
        public BTAction_CheckPlayerInRange(BTHapieTree btParent)
        {
            tree = btParent;
        }

        public override BTNodeState Evaluate()
        {
            if (tree.detectedPlayer)
            {
                return BTNodeState.SUCCESS;
            }

            if (tree.playerTransform == null) 
                return BTNodeState.FAILURE;

            float distance = Vector2.Distance(tree.treeTransform.position, tree.playerTransform.position);

            if (distance <= tree.detectionRadius)
            {
                tree.fxDetectPlayer?.ShowSFX(tree.sfxAttackName);
                tree.lastPlayerPosition = tree.playerTransform.position; 
                tree.target = tree.lastPlayerPosition;
                tree.detectedPlayer = true;
                return BTNodeState.SUCCESS;
            }
            return BTNodeState.FAILURE;
        }
    }
}
