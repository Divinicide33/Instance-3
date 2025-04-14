using BehaviorTree;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_DiveToPlayer : BTNode
    {
        private BTHermesTree tree;
        private float diveSpeed;
        private int platformMask;
        public BTAction_DiveToPlayer(BTHermesTree btParent)
        {
            tree = btParent;
            platformMask = LayerMask.GetMask(LayerMap.Platform.ToString());
            diveSpeed = btParent.diveSpeed;
        }

        public override BTNodeState Evaluate()
        {
            if (tree.player == null) return BTNodeState.FAILURE;

            Vector2 direction = ((Vector2)tree.target - (Vector2)tree.tree.position).normalized;
            tree.gameObject.transform.Translate(direction * diveSpeed * Time.deltaTime);

            RaycastHit2D hitGround = Physics2D.Raycast(tree.tree.position, direction, 2f, platformMask);

            Debug.DrawRay(tree.tree.position, direction * 2, Color.red);

            if (hitGround.collider != null)
            {
                tree.waited = false;
                tree.target = Vector3.zero;
                tree.actionStarted = false;
                tree.action = Action.None;
                tree.charged = false;
                tree.hasJumped = false;
                return BTNodeState.SUCCESS;
            }

            return BTNodeState.RUNNING; // Le boss continue de se déplacer vers le joueur
        }
    }
}
