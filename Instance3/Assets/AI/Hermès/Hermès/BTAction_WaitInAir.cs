using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_WaitInAir : BTNode
    {
        private BTHermesTree tree;
        private float delayTimer;
        private float targetTime;
        private bool hasTarget;
        //private Rigidbody2D rb;



        public BTAction_WaitInAir(BTHermesTree btParent)
        {
            hasTarget = false;
            targetTime = btParent.targetTime;
            tree = btParent;
            delayTimer = btParent.timeInAir;
            //rb = tree.tree.GetComponent<Rigidbody2D>();
        }

        public override BTNodeState Evaluate()
        {
            if (tree.waited)
            {
                return BTNodeState.SUCCESS;
            }

            delayTimer -= Time.deltaTime;

            if (!hasTarget && tree.timeInAir - targetTime < delayTimer)
            {
                //tree.transform.GetChild(1).GetComponent<ArrowForHermesFX>().RotateArrow();
            }
            else if (!hasTarget && tree.timeInAir - targetTime > delayTimer)
            {
                //ArrowForHermesFX.onLockArrow?.Invoke(true);
                hasTarget = true;
                tree.target = tree.player.position;
            }
            if (delayTimer > 0)
            {
                tree.rb.linearVelocity = Vector2.zero;
                tree.rb.simulated = false;
                return BTNodeState.RUNNING;
            }

            if (tree.target == Vector3.zero)
            {
                tree.target = tree.player.position;
            }
            //ArrowForHermesFX.onLockArrow?.Invoke(false);
            tree.waited = true;
            hasTarget = false;
            tree.rb.simulated = true;
            delayTimer = tree.timeInAir;
            return BTNodeState.SUCCESS;
        }
    }
}
