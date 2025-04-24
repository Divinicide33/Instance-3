using BehaviorTree;
using UnityEngine;

namespace AI.Hermes
{
    public class BTAction_DashChance : BTNode
    {
        private BTHermesTree tree;
        private float startTime;
        private bool isWaiting = false;
        private float vfxDuration = 1f;
        private float dashChanceValue;

        public BTAction_DashChance(BTHermesTree btParent)
        {
            tree = btParent;
        }

        public override BTNodeState Evaluate()
        {
            switch (tree.action)
            {
                case Action.None:
                    if (!isWaiting)
                    {
                       tree.fxDetectPlayer.ShowVFX();
                        dashChanceValue = Random.Range(0f, 100f);
                        startTime = Time.time;
                        isWaiting = true;
                        return BTNodeState.RUNNING;
                    }
                    if (Time.time - startTime < vfxDuration)
                    {
                        return BTNodeState.RUNNING;
                    }
                   tree.fxDetectPlayer.HideFX();
                    isWaiting = false;

                    if (dashChanceValue <= tree.dashChance)
                    {
                        tree.action = Action.Dash;
                        return BTNodeState.SUCCESS;
                    }

                    tree.action = Action.Jump;
                    return BTNodeState.FAILURE;

                case Action.Dash:
                    return BTNodeState.SUCCESS;

                case Action.Jump:
                    return BTNodeState.FAILURE;
            }

            return BTNodeState.FAILURE;
        }
    }
}
