using UnityEngine;
using BehaviorTree;

namespace AI.WildBoard
{
    public class BTAction_Patrol : BTNode
    {
        private BTBoarTree tree;

        private float moveSpeed;
        private float detectionDistance = 2f;

        private Vector2 direction;

        private Transform boar;
        private Transform fovOrigin;

        private LayerMask platformLayerMask;

        private bool initialized = false;

        public BTAction_Patrol(BTBoarTree btParent)
        {
            tree = btParent;
            boar = btParent.transform;
            moveSpeed = btParent.moveSpeed;
            fovOrigin = btParent.fovOrigin;
            platformLayerMask = LayerMask.GetMask(LayerMap.Platform.ToString());
        }

        public override BTNodeState Evaluate()
        {
            if (!initialized)
            {
                direction = tree.lastDashDirection != Vector2.zero ? tree.lastDashDirection : Vector2.right;
                bool facingRight = direction.x > 0f;
                boar.eulerAngles = new Vector3(0f, facingRight ? 0f : 180f, 0f);

                initialized = true;
            }
            RaycastHit2D hitObstacle = Physics2D.Raycast(fovOrigin.position, direction, detectionDistance, platformLayerMask);
            if (hitObstacle.collider != null)
            {
                FlipDirection();
            }
            RaycastHit2D hitGround = Physics2D.Raycast(fovOrigin.position, Vector2.down, detectionDistance, platformLayerMask);
            if (hitGround.collider == null)
            {
                
                FlipDirection();
            }
            boar.position += (Vector3)(direction.normalized * moveSpeed * Time.deltaTime);
            Debug.DrawRay(fovOrigin.position, direction * detectionDistance, Color.red);
            Debug.DrawRay(boar.position, Vector2.down * detectionDistance, Color.green);

            state = BTNodeState.SUCCESS;
            return state;
        }
        private void FlipDirection()
        {
            direction *= -1;
            Vector3 currentEuler = boar.eulerAngles;
            boar.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
        }

    }
}
