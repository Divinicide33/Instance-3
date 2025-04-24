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
        private float detectionDistance = 0.7f;

        public BTAction_ChargePattern(BTBoarTree tree)
        {
            this.tree = tree;
            delayTimer = tree.chargeDelay;
            dashTimer = tree.dashDuration;
        }

        public override BTNodeState Evaluate()
        {
            // Phase 1 : Delay avant le dash
            if (!charging)
            {
                tree.fxDetectPlayer.ShowVFX();
                delayTimer -= Time.deltaTime;
                if (delayTimer > 0)
                    return BTNodeState.RUNNING;

                charging = true;
                dashTimer = tree.dashDuration;
            }

            dashTimer -= Time.deltaTime;

            Vector2 raycastOrigin = tree.fovOrigin.transform.position;
            Vector2 raycastDirection = tree.direction;

            Debug.DrawRay(raycastOrigin, raycastDirection * detectionDistance, Color.red);
            RaycastHit2D hitWall = Physics2D.Raycast(raycastOrigin, raycastDirection, detectionDistance, tree.obstacleLayer);

            if (hitWall.collider != null)
            {
                tree.FlipDirection();         
            }        

            Vector2 dashDirection = tree.direction.x >= 0 ? tree.direction : -tree.direction;

            Vector2 dashMovement = dashDirection * tree.dashSpeed * Time.deltaTime;
            tree.dashFeedback.gameObject.SetActive(true);
            //Debug.Log(dashMovement);
            tree.transform.Translate(dashMovement);

            if (dashTimer <= 0)
            {
                ResetCharge();
                return BTNodeState.SUCCESS;
            }
            tree.fxDetectPlayer?.HideFX();
            return BTNodeState.RUNNING;
        }

        private void ResetCharge()
        {
            tree.dashStarted = false;
            charging = false;
            delayTimer = tree.chargeDelay;
            dashTimer = tree.dashDuration;
            tree.dashFeedback.gameObject.SetActive(false);
        }
    }
}
