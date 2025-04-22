using BehaviorTree;
using UnityEngine;

namespace AI.WildBoard
{
    public class BTAction_Patrol : BTNode
    {
        private BTBoarTree tree;
        private float moveSpeed;
        private float detectionDistance = 0.7f;
        private Transform boar;
        private Transform fovOrigin;
        private LayerMask platformLayerMask;

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
            RaycastHit2D hitObstacle = Physics2D.Raycast(fovOrigin.position, tree.direction, detectionDistance, platformLayerMask);
            if (hitObstacle.collider != null)
            {
                tree.FlipDirection();              
            }

            RaycastHit2D hitGround = Physics2D.Raycast(fovOrigin.position, Vector2.down, detectionDistance, platformLayerMask);
            if (hitGround.collider == null)
            {
                tree.FlipDirection();
            }

            boar.position += (Vector3)(tree.direction.normalized * moveSpeed * Time.deltaTime);

            Debug.DrawRay(fovOrigin.position, tree.direction * detectionDistance, Color.red);
            Debug.DrawRay(boar.position, Vector2.down * detectionDistance, Color.green);

            state = BTNodeState.RUNNING;
            return state;
        }
    }
}
