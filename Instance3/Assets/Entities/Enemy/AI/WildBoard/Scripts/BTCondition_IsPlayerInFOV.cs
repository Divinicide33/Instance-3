using UnityEngine;
using BehaviorTree;

namespace AI.WildBoard
{
    public class BTCondition_IsPlayerInFOV : BTNode
    {
        private BTBoarTree tree;

        private float Timer;

        public BTCondition_IsPlayerInFOV(BTBoarTree tree)
        {
            this.tree = tree;
            Timer = tree.DurationEnterDash;
        }

        public override BTNodeState Evaluate()
        {
            if (tree.dashStarted)
            {
                return BTNodeState.SUCCESS;
            }

            Timer += Time.deltaTime;
            if (Timer <= tree.DurationEnterDash)
            {
                return BTNodeState.FAILURE;
            }

            // 1. Détection de zone
            Collider2D[] hits = Physics2D.OverlapCircleAll(tree.fovOrigin.position, tree.detectionRadius, tree.playerLayer);

            if (hits == null || hits.Length == 0)
            {
                //tree.playerIsInFOV = false;
                return BTNodeState.FAILURE;
            }

            // 2. Vérification dans le FOV.
            float halfFovRad = Mathf.Deg2Rad * tree.fovAngle * 0.5f;
            float dotThreshold = Mathf.Cos(halfFovRad);
            Transform target = null;

            foreach (Collider2D hit in hits)
            {
                Vector2 directionToHit = ((Vector2)hit.transform.position - (Vector2)tree.fovOrigin.position).normalized;
                float dot = Vector2.Dot(tree.fovOrigin.right, directionToHit);
                if (dot >= dotThreshold)
                {
                    target = hit.transform;
                    break;
                }
            }
            if (target == null)
            {
                return BTNodeState.FAILURE;
            }

            Vector3 directionToTarget = ((Vector2)target.position - (Vector2)tree.fovOrigin.position).normalized;
            float distanceToTarget = Vector2.Distance(target.position, tree.fovOrigin.position);
            RaycastHit2D rayHit = Physics2D.Raycast(tree.fovOrigin.position, directionToTarget, distanceToTarget, tree.obstacleLayer);

            if (rayHit.collider != null)
            {

                DrawRaySegments2D(tree.fovOrigin.position, directionToTarget, rayHit.distance, distanceToTarget);
                return BTNodeState.FAILURE;
            }

            Debug.DrawRay(tree.fovOrigin.position, directionToTarget * distanceToTarget, Color.green);
            tree.dashStarted = true;
            Timer = 0;
            return BTNodeState.SUCCESS;
        }

        /// <summary>
        /// Dessine deux segments de raycast en 2D :
        /// - Jusqu'à l'obstacle (rouge)
        /// - De l'obstacle jusqu'au candidat (jaune)
        /// </summary>
        /// <param name="origin">Point de départ</param>
        /// <param name="direction">Direction du rayon (normalisée)</param>
        /// <param name="hitDistance">Distance jusqu'à l'obstacle</param>
        /// <param name="totalDistance">Distance totale jusqu'au candidat</param>
        private void DrawRaySegments2D(Vector2 origin, Vector2 direction, float hitDistance, float totalDistance)
        {
            Debug.DrawRay(origin, direction * hitDistance, Color.red);
            Debug.DrawRay(origin + direction * hitDistance, direction * (totalDistance - hitDistance), Color.yellow);
        }
    }
}