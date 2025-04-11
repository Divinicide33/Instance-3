using UnityEngine;
using BehaviorTree;

namespace AI.WildBoard
{
    public class BTAction_Patrol : BTNode
    {
        private Transform _boar;
        private float _moveSpeed;
        private Vector2 _direction;

        private float _detectionDistance = 2f;
        private Transform _fovOrigin;

        private LayerMask _platformLayerMask;

        private bool _initialized = false;

        private BTBoarTree _tree;

        public BTAction_Patrol(BTBoarTree btParent)
        {
            _tree = btParent;
            _boar = btParent.transform;
            _moveSpeed = btParent.moveSpeed;
            _fovOrigin = btParent.fovOrigin;
            _platformLayerMask = LayerMask.GetMask("Platform");
        }

        public override BTNodeState Evaluate()
        {
            if (!_initialized)
            {
                _direction = _tree.lastDashDirection != Vector2.zero ? _tree.lastDashDirection : Vector2.right;
                bool facingRight = _direction.x > 0f;
                _boar.eulerAngles = new Vector3(0f, facingRight ? 0f : 180f, 0f);

                _initialized = true;
            }
            RaycastHit2D hitObstacle = Physics2D.Raycast(_fovOrigin.position, _direction, _detectionDistance, _platformLayerMask);
            if (hitObstacle.collider != null)
            {
                FlipDirection();
            }
            RaycastHit2D hitGround = Physics2D.Raycast(_fovOrigin.position, Vector2.down, _detectionDistance, _platformLayerMask);
            if (hitGround.collider == null)
            {
                
                FlipDirection();
            }
            _boar.position += (Vector3)(_direction.normalized * _moveSpeed * Time.deltaTime);
            Debug.DrawRay(_fovOrigin.position, _direction * _detectionDistance, Color.red);
            Debug.DrawRay(_boar.position, Vector2.down * _detectionDistance, Color.green);

            state = BTNodeState.SUCCESS;
            return state;
        }
        private void FlipDirection()
        {
            _direction *= -1;
            Vector3 currentEuler = _boar.eulerAngles;
            _boar.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
        }

    }
}
