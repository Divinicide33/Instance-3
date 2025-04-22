using BehaviorTree;
using UnityEngine;

namespace AI.Harpie
{
    public class BTAction_DiveToPlayer : BTNode
    {
        private BTHapieTree tree;
        private Transform harpie;
        private Rigidbody2D rb;

        public BTAction_DiveToPlayer(BTHapieTree btParent)
        {
            tree = btParent;
            harpie = tree.tree;
            rb = harpie.GetComponent<Rigidbody2D>();
        }

        public override BTNodeState Evaluate()
        {
            tree.fxDetectPlayer?.ShowVFX();
            tree.transform.position = Vector3.MoveTowards(tree.transform.position, tree.lastPlayerPosition,tree.diveSpeed * Time.deltaTime);
            rb.gravityScale = 0f;

            if (harpie.transform.position == tree.lastPlayerPosition)
            {
                tree.fxDetectPlayer?.HideFX();
                tree.detectedPlayer = false;   
                rb.gravityScale = 0f;
                tree.target = tree.lastPlayerPosition;
                tree.lastDiveTime = Time.time;
                tree.charged = false;
                return BTNodeState.SUCCESS;
            }
            return BTNodeState.RUNNING;
        }
    }
}
