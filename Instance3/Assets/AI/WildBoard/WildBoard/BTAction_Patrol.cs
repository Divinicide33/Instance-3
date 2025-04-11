using UnityEngine;
using BehaviorTree;

namespace AI.WildBoard
{
    public class BTAction_Patrol : BTNode
    {
        private Transform _boar;
        private float _moveSpeed;
        private Vector2 _direction;

        private float _detectionDistance = 0.75f;
        private Transform _fovOrigin;

        private LayerMask _platformLayerMask;

        public BTAction_Patrol(BTBoarTree btParent)
        {
            _boar = btParent.transform;
            _moveSpeed = btParent.moveSpeed;
            _direction = Vector2.right;
            _fovOrigin = btParent.fovOrigin;
            _platformLayerMask = LayerMask.GetMask("Platform");
        }

        public override BTNodeState Evaluate()
        {
            RaycastHit2D hitObstacle =
                Physics2D.Raycast(_fovOrigin.position, _direction, _detectionDistance,
                    _platformLayerMask); // front Raycast
            if (hitObstacle.collider != null)
            {
                _direction *= -1;
                Vector3 currentEuler = _boar.eulerAngles;
                _boar.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
            }

            RaycastHit2D
                hitGround = Physics2D.Raycast(_fovOrigin.position, Vector2.down, _detectionDistance); //floor raycast
            if (hitGround.collider == null)
            {
                _direction *= -1;
                Vector3 currentEuler = _boar.eulerAngles;
                _boar.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
            }

            _boar.position += (Vector3)(_direction.normalized * _moveSpeed * Time.deltaTime);
            Debug.DrawRay(_fovOrigin.position, _direction * _detectionDistance, Color.red);
            Debug.DrawRay(_boar.position, Vector2.down * _detectionDistance, Color.green);

            state = BTNodeState.SUCCESS;
            return state;
        }
    }
}