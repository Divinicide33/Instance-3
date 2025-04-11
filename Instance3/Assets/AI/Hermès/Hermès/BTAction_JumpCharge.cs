using BehaviorTree;
using UnityEngine;

public class BTAction_JumpCharge : BTNode
{
    private HermèsBehaviorTree Tree;
    private float _jumpHeight;
    private float _jumpDuration;
    private float _jumpTime;
    private bool _isJumping;
    private Vector3 _targetPosition;
    private Vector3 _jumpStartPos;

    public BTAction_JumpCharge(HermèsBehaviorTree btParent)
    {
        Tree = btParent;
        _jumpHeight = btParent._jumpHeight;
        _jumpDuration = btParent._jumpDuration;
        _isJumping = false;
    }

    public override BTNodeState Evaluate()
    {
        if (!_isJumping)
        {
            _targetPosition = Tree.player.position;
            _jumpStartPos = Tree.Tree.position;

            _jumpTime = 0f;
            _isJumping = true;
        }
        _jumpTime += Time.deltaTime;

        if (_jumpTime < _jumpDuration)
        {
            float jumpProgress = _jumpTime / _jumpDuration;
            float jumpHeight = Mathf.Sin(jumpProgress * Mathf.PI) * _jumpHeight;

            Vector3 newPosition = Vector3.Lerp(_jumpStartPos, _targetPosition, jumpProgress);
            newPosition.y += jumpHeight;

            Tree.Tree.position = newPosition;

            return BTNodeState.RUNNING; 
        }
        Tree.Tree.position = _targetPosition;

        Tree.Action = HermèsBehaviorTree.HermesAction.None;
        Tree.actionStarted = false;
        _isJumping = false ;
        return BTNodeState.SUCCESS;
    }
}
