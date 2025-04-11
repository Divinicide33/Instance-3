using AI.WildBoard;
using BehaviorTree;
using UnityEngine;

public class BTAction_DashHermes : BTNode
{
    private HermèsBehaviorTree Tree;
    private bool charging = false;
    private float dashTimer;
    private float delayTimer;
    private Vector2 _direction;

    public BTAction_DashHermes(HermèsBehaviorTree btParent)
    {
        this.Tree = btParent;
        delayTimer = btParent.chargeDelay;
        dashTimer = btParent.dashDuration;
    }

    public override BTNodeState Evaluate()
    {
        // Phase 2 : Dash
        dashTimer -= Time.deltaTime;

        Vector2 dashDirection = new Vector2(Tree.dashSpeed * Time.deltaTime, 0); // On suppose ici une direction horizontale
        Tree.gameObject.transform.Translate(dashDirection);

        // Enregistrer la direction du dash
        Tree.lastDashDirection = dashDirection.normalized;

        // Détection de collision avec le joueur et knockback
        Collider2D hitPlayer = Physics2D.OverlapCircle(Tree.gameObject.transform.position, 0.7f, Tree.playerLayer);
        if (hitPlayer != null)
        {
            Rigidbody2D rb = hitPlayer.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 knockbackDir = ((Vector2)hitPlayer.transform.position - (Vector2)Tree.gameObject.transform.position);
                knockbackDir.y = 0;
                knockbackDir.Normalize();
                knockbackDir.y = knockbackDir.magnitude;
                rb.linearVelocity = knockbackDir * Tree.knockbackForce;
                //invincibilter.invoke() !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
        }

        // CHANGE DIRECTION WHEN WALL !!!!!!!!!!!!!!!!!!!!!

        if (dashTimer > 0)
        {
            return BTNodeState.RUNNING;
        }

        ResetCharge();
        return BTNodeState.SUCCESS;
    }

    private void ResetCharge()
    {
        Tree.actionStarted = false;
        charging = false;
        delayTimer = Tree.chargeDelay;
        dashTimer = Tree.dashDuration;
        Tree.Action = HermèsBehaviorTree.HermesAction.None;
    }
}
