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
            Vector3 directionToOrigin = tree.origin - harpie.position;
            if (directionToOrigin.x > 0)
            {
                harpie.localScale = new Vector3(-Mathf.Abs(harpie.localScale.x), harpie.localScale.y, harpie.localScale.z);
            }
            else
            {
                harpie.localScale = new Vector3(Mathf.Abs(harpie.localScale.x), harpie.localScale.y, harpie.localScale.z);
            }
            tree.transform.position = Vector3.MoveTowards(tree.transform.position, tree.origin, tree.returnSpeed * Time.deltaTime);

            return BTNodeState.SUCCESS;
        }
    }
}
