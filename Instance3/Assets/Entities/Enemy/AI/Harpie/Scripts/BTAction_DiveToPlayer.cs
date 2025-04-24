using BehaviorTree;
using UnityEngine;

namespace AI.Harpie
{
    public class BTAction_DiveToPlayer : BTNode
    {
        private BTHapieTree tree;
        private Transform harpie;

        public BTAction_DiveToPlayer(BTHapieTree btParent)
        {
            tree = btParent;
            harpie = tree.treeTransform;
        }
        public override BTNodeState Evaluate()
        {
            if (tree.lastPlayerPosition.x > harpie.position.x)
            {
                harpie.localScale = new Vector3(-Mathf.Abs(tree.initialScale.x), tree.initialScale.y, tree.initialScale.z); 
            }
            else
            {
                harpie.localScale = new Vector3(Mathf.Abs(tree.initialScale.x), tree.initialScale.y, tree.initialScale.z); 
            }

            tree.dashFeedback.gameObject.SetActive(true);
            tree.transform.position = Vector3.MoveTowards(tree.transform.position, tree.lastPlayerPosition, tree.diveSpeed * Time.deltaTime);
            tree.rb.gravityScale = 0f;

            if (harpie.transform.position == tree.lastPlayerPosition)
            {
                tree.dashFeedback.gameObject.SetActive(false);
                tree.detectedPlayer = false;
                tree.rb.gravityScale = 0f;
                tree.target = tree.lastPlayerPosition;
                tree.lastDiveTime = Time.time;
                tree.charged = false;
                return BTNodeState.SUCCESS;
            }

            return BTNodeState.RUNNING;
        }
    }
}
