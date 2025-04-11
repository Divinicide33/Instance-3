using UnityEngine;
using BehaviorTree;

namespace AI.WildBoard
{
    public class BTAction_MoveToTarget : BTNode
    {
        public Transform _entityTransform;
        public Transform _targetTransform;
        public float _moveSpeed;

        public BTAction_MoveToTarget(BTBoarTree btParent)
        {
            _entityTransform = btParent.transform;
            _targetTransform = btParent.transform.parent;
            _moveSpeed = btParent.moveSpeed;
        }

        public override BTNodeState Evaluate()
        {
            if (_targetTransform != null)
            {
                _entityTransform.position = Vector2.MoveTowards(_entityTransform.position, _targetTransform.position,
                    _moveSpeed * Time.deltaTime);
                float distance = Vector2.Distance(_entityTransform.position, _targetTransform.position);

                if (distance < 0.1f)
                {
                    return BTNodeState.SUCCESS;
                }

                return BTNodeState.RUNNING;
            }

            return BTNodeState.FAILURE;
        }
    }
}