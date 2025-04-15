//using BehaviorTree;
//using UnityEngine;

//namespace AI.Hermes
//{
//    public class BTAction_JumpCharge : BTNode
//    {
//        private BTHermesTree tree;
//        private float jumpHeight;
//        private float jumpDuration;
//        private float jumpTime;
//        private bool isJumping;
//        private Vector3 tragetPosition;
//        private Vector3 jumpStartPos;

//        public BTAction_JumpCharge(BTHermesTree btParent)
//        {
//            tree = btParent;
//            jumpHeight = btParent.jumpHeight;
//            jumpDuration = btParent.jumpDuration;
//            isJumping = false;
//        }

//        public override BTNodeState Evaluate()
//        {
//            if (!isJumping)
//            {
//                tragetPosition = tree.player.position;
//                jumpStartPos = tree.tree.position;

//                jumpTime = 0f;
//                isJumping = true;
//            }
//            jumpTime += Time.deltaTime;

//            if (jumpTime < jumpDuration)
//            {
//                float jumpProgress = jumpTime / jumpDuration;
//                float jumpHeight = Mathf.Sin(jumpProgress * Mathf.PI) * this.jumpHeight;

//                Vector3 newPosition = Vector3.Lerp(jumpStartPos, tragetPosition, jumpProgress);
//                newPosition.y += jumpHeight;

//                tree.tree.position = newPosition;

//                return BTNodeState.RUNNING;
//            }
//            tree.tree.position = tragetPosition;

//            tree.action = Action.None;
//            tree.actionStarted = false;
//            isJumping = false;
//            tree.charged = false;
//            return BTNodeState.SUCCESS;
//        }
//    }
//}
