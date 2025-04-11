using BehaviorTree;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class BTAction_Delay : BTNode
{
    private HermèsBehaviorTree Hermes;

    private bool charged = false;
    private float dashTimer;
    private float delayTimer;

    public BTAction_Delay(HermèsBehaviorTree btParent)
    {
        Hermes = btParent;
        delayTimer = btParent.chargeDelay;
        dashTimer = btParent.dashDuration;
    }

    public override BTNodeState Evaluate()
    {
        // Phase 1 : Charge Delay
        if (charged)
        {
            return BTNodeState.SUCCESS; 
        }

        delayTimer -= Time.deltaTime;
        
        if (delayTimer > 0)
        {
            return BTNodeState.RUNNING;
        }

        charged = true;
        dashTimer = Hermes.dashDuration;

        return BTNodeState.SUCCESS;
    }
}
