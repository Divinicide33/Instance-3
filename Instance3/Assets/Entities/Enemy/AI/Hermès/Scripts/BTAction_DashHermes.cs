using BehaviorTree;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_DashHermes : BTNode
    {
        private BTHermesTree tree;
        private float dashTimer;
        private Vector2 direction;
        private Transform raycastTransform;
        private float detectionDistance = 0.5f;

        private float flipCoolDown = 0.2f;
        private float lastFlipTime = -1f;

        private int platformMask;

        float test;

        public BTAction_DashHermes(BTHermesTree btParent)
        {
            this.tree = btParent;
            platformMask = LayerMask.GetMask(LayerMap.Platform.ToString());
            dashTimer = btParent.dashDuration;
            test = dashTimer;
            direction = Vector2.right;
            raycastTransform = btParent.raycast;
        }

        public override BTNodeState Evaluate()
        {
            if (dashTimer == test)
            {
                tree.hermesDashFX?.ShowSFX(tree.sfxDashName);
            }
            dashTimer -= Time.deltaTime;

            Vector2 raycastDirection = tree.tree.localScale.x >= 0 ? Vector2.right : Vector2.left;
            direction = raycastDirection;

            RaycastHit2D hitWall = Physics2D.Raycast(raycastTransform.position, raycastDirection, detectionDistance, platformMask);

            Debug.DrawRay(raycastTransform.position, raycastDirection * detectionDistance, Color.red);

            if (hitWall.collider != null && Time.time - lastFlipTime >= flipCoolDown)
            {
                tree.FlipDirection(ref direction);
                lastFlipTime = Time.time;
                //Debug.Log("Collision d�tect�e avec : " + hitWall.collider.name);
            }

            Vector2 dashMovement = direction * tree.dashSpeed * Time.deltaTime;
            tree.gameObject.transform.Translate(dashMovement);

            tree.lastDashDirection = direction;

            if (dashTimer <= 0)
            {
                ResetCharge();
                return BTNodeState.SUCCESS;
            }

            return BTNodeState.RUNNING;
        }

        private void ResetCharge()
        {
            tree.actionStarted = false;
            dashTimer = tree.dashDuration;
            tree.action = Action.None;
            tree.charged = false;
        }
    }
}
