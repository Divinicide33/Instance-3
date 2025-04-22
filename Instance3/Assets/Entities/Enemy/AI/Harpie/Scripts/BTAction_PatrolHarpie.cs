using BehaviorTree;
using UnityEngine;

namespace AI.Harpie
{
    public class BTAction_PatrolHarpie : BTNode
    {
        private BTHapieTree tree;
        private Transform harpie;
        private Rigidbody2D rb;

        public BTAction_PatrolHarpie(BTHapieTree btParent)
        {
            tree = btParent;
            harpie = tree.treeTransform;
            rb = harpie.GetComponent<Rigidbody2D>();
        }

        public override BTNodeState Evaluate()
        {
            harpie.rotation = tree.idleRotation;
            tree.transform.position = Vector3.MoveTowards(tree.transform.position, tree.origin, tree.returnSpeed * Time.deltaTime);
            return BTNodeState.SUCCESS;
        }
    }
}
