using UnityEngine;
using BehaviorTree;


public class BTAction_MoveToTarget : BTNode
{
    private Transform _entityTransform;
    private Transform _targetTransform;
    private float _moveSpeed;

    public BTAction_MoveToTarget(Transform entity, Transform target, float moveSpeed)
    {
        _entityTransform = entity;
        _targetTransform = target;
        _moveSpeed = moveSpeed;
    }

    public override BTNodeState Evaluate()
    {
        if (_targetTransform != null)
        {
            _entityTransform.position = Vector2.MoveTowards(_entityTransform.position, _targetTransform.position, _moveSpeed * Time.deltaTime);
            float distance = Vector2.Distance(_entityTransform.position, _targetTransform.position);

            if (distance < 0.1f)
            {
                return BTNodeState.SUCCESS;  // Arrivé à la destination
            }

            return BTNodeState.RUNNING;  // Toujours en mouvement
        }

        return BTNodeState.FAILURE;  // Pas de cible
    }
}

