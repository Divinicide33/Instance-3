using UnityEngine;
using BehaviorTree;

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
                delayTimer -= Time.deltaTime;
                //Debug.Log($"Charge Delay Phase 1: {delayTimer:F2}/{tree.chargeDelay}");
                if (delayTimer > 0)
                {
                    return BTNodeState.RUNNING;
                }
                
                charging = true; // Le délai est écoulé, on passe à la phase de dash
                dashTimer = tree.dashDuration;
                //Debug.Log("Charge delay terminé. Démarrage du dash.");
            }

            // Phase 2 : Dash
            dashTimer -= Time.deltaTime;
            //Debug.Log($"Charge Delay Phase 2: {delayTimer:F2}/{tree.chargeDelay}");
            
            float jumpForce = 2f; // Valeur ajustable pour le saut
            Vector2 movement = new Vector2(tree.dashSpeed * Time.deltaTime, jumpForce * Time.deltaTime);
            tree.gameObject.transform.Translate(movement);

            // Détecter une collision avec le joueur dans un petit cercle autour de l'IA (pour appliquer un knockback)
            Collider2D hitPlayer = Physics2D.OverlapCircle(tree.gameObject.transform.position, 0.7f, tree.playerLayer);
            if (hitPlayer != null)
            {
                Rigidbody2D rb = hitPlayer.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 knockbackDir = ((Vector2)hitPlayer.transform.position - (Vector2)tree.gameObject.transform.position).normalized;
                    rb.linearVelocity = knockbackDir * tree.knockbackForce;
                }
            }
            
            if (dashTimer > 0)
            {
                return BTNodeState.RUNNING;
            }
            
            ResetCharge();
            return BTNodeState.SUCCESS;
        }
        
        /// <summary>
        /// Réinitialise les variables internes et efface la donnée "playerIsInFOV"
        /// afin de préparer la prochaine charge.
        /// </summary>
        private void ResetCharge()
        {
            tree.dashStarted = false;
            charging = false;
            delayTimer = tree.chargeDelay;
            dashTimer = tree.dashDuration;
            //Debug.Log("Pattern de charge terminé. Les compteurs sont réinitialisés.");
        }
    }
}