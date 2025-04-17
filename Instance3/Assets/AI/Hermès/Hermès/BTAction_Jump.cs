using BehaviorTree;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_Jump : BTNode
    {
        private BTHermesTree tree;
        private Transform targetTransform;
        private float jumpSpeed;

        public BTAction_Jump(BTHermesTree btParent)
        {
            tree = btParent;
            targetTransform = btParent.targetTransform;
            jumpSpeed = btParent.jumpSpeed;
        }

        public override BTNodeState Evaluate()
        {
            if (tree.hasJumped)
            {
                return BTNodeState.SUCCESS;
            }

            Vector2 direction = (targetTransform.position - tree.tree.position);

            if (direction.magnitude > 0.1f)
            {
                tree.rb.simulated = false;
                tree.transform.position = Vector3.MoveTowards(tree.tree.position, targetTransform.position, jumpSpeed * Time.deltaTime);
                //tree.tree.position += (Vector3)(direction.normalized * jumpSpeed * Time.deltaTime);
                return BTNodeState.RUNNING;
            }

            tree.hasJumped = true;
            return BTNodeState.SUCCESS;
        }
    }
}
