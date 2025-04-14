using BehaviorTree;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_Cooldown : BTNode
    {
        private BTHermesTree tree;
        private float coolDownTime;
        private float elapsedTime;

        public BTAction_Cooldown(BTHermesTree btParent)
        {
            tree = btParent;
            coolDownTime = btParent.coolDownEnterDash;
            elapsedTime = coolDownTime;
        }

        public override BTNodeState Evaluate()
        {
            if (tree.actionStarted)
            {
                return BTNodeState.SUCCESS;
            }
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= coolDownTime)
            {
                elapsedTime = 0f;
                tree.actionStarted = true;
                return BTNodeState.SUCCESS;
            }
            tree.actionStarted = false;
            return BTNodeState.FAILURE;
        }
    }
}
