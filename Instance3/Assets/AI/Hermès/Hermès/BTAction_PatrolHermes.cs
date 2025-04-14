using UnityEngine;
using BehaviorTree;

namespace AI.Hermes
{
    public class BTAction_PatrolHermes : BTNode
    {
        private BTHermesTree tree;
        private Vector2 direction;
        private float detectionDistance = 0.5f;
        private Transform raycastTransform;

        private int platformMask;

        public BTAction_PatrolHermes(BTHermesTree btParent)
        {
            tree = btParent;
            platformMask = LayerMask.GetMask(LayerMap.Platform.ToString());
            raycastTransform = tree.raycast;
        }

        public override BTNodeState Evaluate()
        {
            direction = tree.lastDashDirection.x >= 0 ? Vector2.right : Vector2.left;
            Vector3 scale = tree.tree.localScale;
            scale.x = direction.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            tree.tree.localScale = scale;

            RaycastHit2D hitObstacle = Physics2D.Raycast(raycastTransform.position, direction, detectionDistance, platformMask);
            Debug.DrawRay(raycastTransform.position, direction * detectionDistance, Color.green);

            if (hitObstacle.collider != null)
            {
                tree.FlipDirection(ref direction);
                tree.lastDashDirection = direction;
            }

            // Mouvement
            tree.tree.position += (Vector3)(direction.normalized * tree.speed * Time.deltaTime);

            return BTNodeState.RUNNING;
        }
    }
}
