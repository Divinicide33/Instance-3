using BehaviorTree;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_WaitInAir : BTNode
    {
        private BTHermesTree tree;
        private float delayTimer;
        private float targetTime;
        private bool hasTarget;
        private Rigidbody2D rb;

        public BTAction_WaitInAir(BTHermesTree btParent)
        {
            hasTarget = false;
            targetTime = btParent.targetTime;
            tree = btParent;
            delayTimer = btParent.timeInAir;
            rb = tree.tree.GetComponent<Rigidbody2D>();
        }

        public override BTNodeState Evaluate()
        {
            if (tree.waited)
            {
                return BTNodeState.SUCCESS;
            }

            delayTimer -= Time.deltaTime;

            if (!hasTarget && tree.timeInAir - targetTime > delayTimer)
            {
                hasTarget = true;
                tree.target = tree.player.position;
            }

            if (delayTimer > 0)
            {
                rb.linearVelocity = Vector2.zero;
                rb.simulated = false;
                return BTNodeState.RUNNING;
            }

            if (tree.target == Vector3.zero)
            {
                tree.target = tree.player.position;
            }

            tree.waited = true;
            hasTarget = false;
            rb.simulated = true;
            delayTimer = tree.timeInAir;
            return BTNodeState.SUCCESS;
        }
    }
}
