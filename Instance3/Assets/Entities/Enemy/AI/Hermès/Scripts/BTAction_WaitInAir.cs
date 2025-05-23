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
                tree.arrowForHermesFX?.ShowVFX();
            }
            else if (!hasTarget && tree.timeInAir - targetTime > delayTimer)
            {
                tree.arrowForHermesFX?.HideFX();
                hasTarget = true;
                tree.target = tree.player.position;
            }
            if (delayTimer > 0)
            {
                tree.rb.linearVelocity = Vector2.zero;
                tree.rb.gravityScale = 0f;
                return BTNodeState.RUNNING;
            }

            if (tree.target == Vector3.zero)
            {
                tree.target = tree.player.position;
            }
            tree.hermesDiveFX?.ShowSFX(tree.sfxDiveName);
            tree.arrowForHermesFX?.HideFX();
            tree.waited = true;
            hasTarget = false;
            tree.rb.gravityScale = 1f;
            delayTimer = tree.timeInAir;
            return BTNodeState.SUCCESS;
        }
    }
}
