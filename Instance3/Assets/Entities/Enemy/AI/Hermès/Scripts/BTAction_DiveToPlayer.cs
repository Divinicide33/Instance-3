using BehaviorTree;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_DiveToPlayer : BTNode
    {
        private BTHermesTree tree;
        private float diveSpeed;
        private int platformMask;
        private Vector3 direction = Vector3.zero;
        private bool hasDirection = false;

        public BTAction_DiveToPlayer(BTHermesTree btParent)
        {
            tree = btParent;
            platformMask = LayerMask.GetMask(LayerMap.Platform.ToString());
            diveSpeed = btParent.diveSpeed;
        }

        public override BTNodeState Evaluate()
        {
            if (tree.player == null) return BTNodeState.FAILURE;

            if (!hasDirection)
            {
                direction = ((Vector2)tree.target - (Vector2)tree.tree.position).normalized;
                hasDirection = true;
            }
            if (direction.x > 0)
            {
                tree.transform.localScale = new Vector3(-Mathf.Abs(tree.initialScale.x), tree.initialScale.y, tree.initialScale.z);
            }
            else
            {
                tree.transform.localScale = new Vector3(Mathf.Abs(tree.initialScale.x), tree.initialScale.y, tree.initialScale.z);
            }

            tree.dashFeedback.gameObject.SetActive(true);
            tree.gameObject.transform.Translate(direction * diveSpeed * Time.deltaTime);

            RaycastHit2D hitGround = Physics2D.Raycast(tree.tree.position, direction, 2f, platformMask);
            Debug.DrawRay(tree.tree.position, direction * 2, Color.red);

            if (hitGround.collider != null)
            {
                tree.dashFeedback.gameObject.SetActive(false);
                tree.waited = false;
                tree.target = Vector3.zero;
                tree.actionStarted = false;
                tree.action = Action.None;
                tree.charged = false;
                tree.hasJumped = false;
                hasDirection = false;
                tree.rb.simulated = true;

                tree.lastDashDirection = direction;

                return BTNodeState.SUCCESS;
            }

            return BTNodeState.RUNNING;
        }
    }
}
