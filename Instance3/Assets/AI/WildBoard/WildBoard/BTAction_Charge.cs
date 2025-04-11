using UnityEngine;
using BehaviorTree;

namespace AI.WildBoard
{
    public class BTAction_Charge : BTNode
    {
        private Transform _boar;
        private GameObject _target;
        private float _chargeDelay;
        private float _dashSpeed;
        private float _dashDuration;
        private float _timer = 0f;
        private bool _charging = false;
        private float _dashTimer = 0f;
        private Vector3 _targetPosition;

        private bool _hasTarget = false;
        private LayerMask _playerLayer;
        private LayerMask _obstacleLayer;
        private float _knockbackForce;

        public bool IsCharging() => _charging || _hasTarget;

        public BTAction_Charge(BTBoarTree btParent, Transform boar)
        {
            _boar = boar;
            _chargeDelay = btParent.chargeDelay;
            _dashSpeed = btParent.dashSpeed;
            _dashDuration = btParent.dashDuration;
            _playerLayer = btParent.playerLayer;
            _obstacleLayer = btParent.obstacleLayer;
            _knockbackForce = btParent.knockbackForce;
        }

        public override BTNodeState Evaluate()
        {
            /*if (root.target == Vector3.zero)
            {
                state = BTNodeState.FAILURE;
                return state;
            }


            if (!_charging)
            {
                _timer += Time.deltaTime;
                if (_timer >= _chargeDelay)
                {
                    _charging = true;
                    _dashTimer = 0f;
                }

                state = BTNodeState.RUNNING;
                return state;
            }
            else
            {
                _dashTimer += Time.deltaTime;
                Collider2D hitPlayer = Physics2D.OverlapCircle(_boar.position, 0.5f, _playerLayer);
                if (hitPlayer != null)
                {
                    var rb = hitPlayer.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        //REMOVE Y
                        Vector2 knockbackDir = (hitPlayer.transform.position - _boar.position).normalized;
                        rb.linearVelocity = knockbackDir * _knockbackForce;
                        //rb.AddForce(knockbackDir * _knockbackForce, ForceMode2D.Impulse);
                    }
                }

                if (_dashTimer >= _dashDuration)
                {
                    _charging = false;
                    root.target = Vector3.zero;
                    _timer = 0f;
                    state = BTNodeState.SUCCESS;
                    return state;
                }
            }
                */

            state = BTNodeState.RUNNING;
            return state;
        }
    }
}