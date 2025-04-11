using UnityEngine;
using BehaviorTree;

namespace AI.WildBoard
{
    public class BTCondition_IsPlayerInFOV : BTNode
    {
        private BTBoarTree tree;
        
        public BTCondition_IsPlayerInFOV(BTBoarTree tree)
        {
            this.tree = tree;
        }

        public override BTNodeState Evaluate()
        {
            // Si le dash est déjà en cours, on ne fait pas de détection.
            if (tree.dashStarted)
            {
                return BTNodeState.SUCCESS;
            }
            
            // 1. Détection de zone
            Collider2D[] hits = Physics2D.OverlapCircleAll(tree.fovOrigin.position, tree.detectionRadius, tree.playerLayer);
            //Debug.Log($"OverlapSphere : {hits.Length} objets détectés dans la zone de détection.");
            
            if (hits == null || hits.Length == 0)
            {
                //tree.playerIsInFOV = false;
                return BTNodeState.FAILURE;
            }

            // 2. Vérification dans le FOV.
            // Calcul de l'angle seuil basé sur le dot product.
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
                //tree.playerIsInFOV = false;
                return BTNodeState.FAILURE;
            }

            // 3. Raycast en direction du candidat/target et Debug.DrawRay.
            Vector3 directionToTarget = ((Vector2)target.position - (Vector2)tree.fovOrigin.position).normalized;
            float distanceToTarget = Vector2.Distance(target.position, tree.fovOrigin.position);
            RaycastHit2D rayHit = Physics2D.Raycast(tree.fovOrigin.position, directionToTarget, distanceToTarget, tree.obstacleLayer);
            
            if (rayHit.collider != null)
            {
                // Si un obstacle est rencontré avant d'atteindre le candidat/target :
                DrawRaySegments2D(tree.fovOrigin.position, directionToTarget, rayHit.distance, distanceToTarget);
                //ClearData("playerIsInFOV");
                return BTNodeState.FAILURE;
            }
            
            Debug.DrawRay(tree.fovOrigin.position, directionToTarget * distanceToTarget, Color.green);
            //tree.playerIsInFOV = true;
            tree.dashStarted = true;
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