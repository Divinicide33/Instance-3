using UnityEngine;
using BehaviorTree;

public class BTAction_Charge : BTNode
{
    public Transform _boar;
    public GameObject _target;
    public float _chargeDelay;
    public float _dashSpeed;
    public float _dashDuration;
    public float _timer = 0f;
    public bool _charging = false;
    public float _dashTimer = 0f;
    public Vector3 _targetPosition;

    public bool _hasTarget = false;
    public LayerMask _playerLayer;
    public LayerMask _obstacleLayer;
    public float _knockbackForce;

    public bool IsCharging() => _charging || _hasTarget;

    public BTAction_Charge(BTBoarTree btParent, Transform boar)
    {
        _boar = boar;
        _target = btParent.player;
        _chargeDelay = btParent.chargeDelay;
        _dashSpeed = btParent.dashSpeed;
        _dashDuration = btParent.dashDuration;
        _playerLayer = btParent.playerLayer;
        _obstacleLayer = btParent.obstacleLayer;
        _knockbackForce = btParent.knockbackForce;
    }

    public override BTNodeState Evaluate()
    {
        if (root.target == Vector3.zero)
        {
            _state = BTNodeState.FAILURE;
            return _state;
        }


        if (!_charging)
        {
            _timer += Time.deltaTime;
            if (_timer >= _chargeDelay)
            {
                _charging = true;
                _dashTimer = 0f;
            }
            _state = BTNodeState.RUNNING;
            return _state;
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
                _state = BTNodeState.SUCCESS;
                return _state;
            }
            _state = BTNodeState.RUNNING;
            return _state;
        }
    }
}
