using BehaviorTree;
using UnityEngine;

public class BTAction_Cooldown : BTNode
{
    private HermèsBehaviorTree Tree;
    private float _cooldownTime;
    private float _elapsedTime;

    public BTAction_Cooldown(HermèsBehaviorTree btParent)
    {
        Tree = btParent;
        _cooldownTime = btParent.CoolDownEnterDash;
        _elapsedTime = _cooldownTime;
    }

    public override BTNodeState Evaluate()
    {
        if (Tree.actionStarted)
        {
            return BTNodeState.SUCCESS;
        }
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _cooldownTime)
        {
            _elapsedTime = 0f;
            Tree.actionStarted = true;
            return BTNodeState.SUCCESS; 
        }
        return BTNodeState.FAILURE;
    }
}
