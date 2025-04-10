using UnityEngine;
using BehaviorTree;

public class BTAction_Patrol : BTNode
{
    public Transform _boar;               
    public float _moveSpeed;               
    public GameObject _player;             
    public Vector2 _direction;            

    public float _detectionDistance = 0.75f; 
    public Transform _fovOrigin;        

    public LayerMask _platformLayerMask;   

    public BTAction_Patrol(BTBoarTree btParent)
    {
        _boar = btParent.transform;
        _moveSpeed = btParent.moveSpeed;
        _player = btParent.player;
        _direction = Vector2.right;  
        _fovOrigin = btParent.fovOrigin;
        _platformLayerMask = LayerMask.GetMask("Platform");
    }

    public override BTNodeState Evaluate()
    {
        RaycastHit2D hitObstacle = Physics2D.Raycast(_fovOrigin.position, _direction, _detectionDistance, _platformLayerMask); // front Raycast
        if (hitObstacle.collider != null)
        {
            _direction *= -1;
            Vector3 currentEuler = _boar.eulerAngles;
            _boar.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
        }
        RaycastHit2D hitGround = Physics2D.Raycast(_fovOrigin.position, Vector2.down, _detectionDistance);//floor raycast
        if (hitGround.collider == null)
        {
            _direction *= -1;
            Vector3 currentEuler = _boar.eulerAngles;
            _boar.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
        }
        _boar.position += (Vector3)(_direction.normalized * _moveSpeed * Time.deltaTime);
        Debug.DrawRay(_fovOrigin.position, _direction * _detectionDistance, Color.red);
        Debug.DrawRay(_boar.position, Vector2.down * _detectionDistance, Color.green);
        
        _state = BTNodeState.SUCCESS;
        return _state;
    }
}
