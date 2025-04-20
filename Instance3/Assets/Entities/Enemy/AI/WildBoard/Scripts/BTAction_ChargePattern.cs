using BehaviorTree;
using UnityEngine;

namespace AI.WildBoard
{
    public class BTAction_ChargePattern : BTNode
    {
        private BTBoarTree tree;

        private bool charging = false;
        private float dashTimer;
        private float delayTimer;

        public BTAction_ChargePattern(BTBoarTree tree)
        {
            this.tree = tree;
            delayTimer = tree.chargeDelay;
            dashTimer = tree.dashDuration;
        }

        public override BTNodeState Evaluate()
        {
            // Phase 1 : Charge Delay
            if (!charging)
            {
                tree.fxDetectPlayer?.ShowFX();
                delayTimer -= Time.deltaTime;
                if (delayTimer > 0)
                {
                    return BTNodeState.RUNNING;
                }

                charging = true;
                dashTimer = tree.dashDuration;
            }

            dashTimer -= Time.deltaTime;

            Vector2 dashDirection = new Vector2(tree.dashSpeed * Time.deltaTime, 0); 
            tree.gameObject.transform.Translate(dashDirection);

            tree.lastDashDirection = dashDirection.normalized;

            Collider2D hitPlayer = Physics2D.OverlapCircle(tree.gameObject.transform.position, 0.7f, tree.playerLayer);
            if (hitPlayer != null)
            {
                Rigidbody2D rb = hitPlayer.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
/*                    Vector2 knockbackDir = ((Vector2)hitPlayer.transform.position - (Vector2)tree.gameObject.transform.position);
                    knockbackDir.y = 0;
                    knockbackDir.Normalize();
                    knockbackDir.y = knockbackDir.magnitude;
                    rb.linearVelocity = knockbackDir * tree.knockbackForce;*/
                    //invincibilter.invoke() !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                }
            }

            if (dashTimer > 0)
            {
                tree.fxDetectPlayer?.HideFX();
                return BTNodeState.RUNNING;
            }

            ResetCharge();
            return BTNodeState.SUCCESS;
        }

        private void ResetCharge()
        {
            tree.dashStarted = false;
            charging = false;
            delayTimer = tree.chargeDelay;
            dashTimer = tree.dashDuration;
        }
    }
}
