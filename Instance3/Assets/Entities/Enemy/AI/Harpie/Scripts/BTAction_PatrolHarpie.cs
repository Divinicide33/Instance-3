using BehaviorTree;
using UnityEngine;

namespace AI.Harpie
{
    public class BTAction_PatrolHarpie : BTNode
    {
        private BTHapieTree tree;
        private Transform harpie;

        public BTAction_PatrolHarpie(BTHapieTree btParent)
        {
            tree = btParent;
            harpie = tree.treeTransform;
        }

        public override BTNodeState Evaluate()
        {
            harpie.rotation = tree.idleRotation;
            tree.transform.position = Vector3.MoveTowards(tree.transform.position, tree.origin, tree.returnSpeed * Time.deltaTime);
            return BTNodeState.SUCCESS;
        }
    }
}
