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
            // Utiliser la direction du dernier dash ou la direction par d�faut
            if (!_initialized)
            {
                _direction = _tree.lastDashDirection != Vector2.zero ? _tree.lastDashDirection : Vector2.right;

                // Initialiser la direction du sanglier en fonction de celle du dash
                bool facingRight = _direction.x > 0f;
                _boar.eulerAngles = new Vector3(0f, facingRight ? 0f : 180f, 0f);

                _initialized = true;
            }

            // Raycast pour d�tecter les obstacles devant (mur)
            RaycastHit2D hitObstacle = Physics2D.Raycast(_fovOrigin.position, _direction, _detectionDistance, _platformLayerMask);
            if (hitObstacle.collider != null)
            {
                // Si un obstacle est d�tect�, changer de direction
                FlipDirection();
            }

            // Raycast pour d�tecter le sol (�viter les chutes)
            RaycastHit2D hitGround = Physics2D.Raycast(_fovOrigin.position, Vector2.down, _detectionDistance, _platformLayerMask);
            if (hitGround.collider == null)
            {
                // Si il n'y a plus de sol, changer de direction
                FlipDirection();
            }

            // D�placer le sanglier
            _boar.position += (Vector3)(_direction.normalized * _moveSpeed * Time.deltaTime);

            // Debug pour voir les raycasts dans l'�diteur
            Debug.DrawRay(_fovOrigin.position, _direction * _detectionDistance, Color.red);
            Debug.DrawRay(_boar.position, Vector2.down * _detectionDistance, Color.green);

            state = BTNodeState.SUCCESS;
            return state;
        }

        /// <summary>
        /// Inverse la direction du sanglier et ajuste son orientation visuelle.
        /// </summary>
        private void FlipDirection()
        {
            _direction *= -1;
            Vector3 currentEuler = _boar.eulerAngles;
            _boar.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
        }

    }
}
