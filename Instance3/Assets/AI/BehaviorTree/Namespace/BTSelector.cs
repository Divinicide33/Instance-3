using System.Collections.Generic;

namespace BehaviorTree
{
    public class BTSelector : BTNode
    {
        // Constructor for a selector node without children
        public BTSelector() : base() { }

        // Constructor for a selector node with children
        public BTSelector(List<BTNode> children) : base(children) { }

        // Evaluate the selector node
        public override BTNodeState Evaluate()
        {
            // Iterate through each child node
            foreach (BTNode BTNode in children)
            {
                switch (BTNode.Evaluate())
                {
                    case BTNodeState.FAILURE:
                        continue;  // Move to the next child if the current one fails

                    case BTNodeState.SUCCESS:
                        state = BTNodeState.SUCCESS;
                        return state;  // Return SUCCESS if any child succeeds

                    case BTNodeState.RUNNING:
                        state = BTNodeState.RUNNING;
                        return state;  // Return RUNNING if any child is still running

                    default:
                        continue;  // Continue to the next child for any other state
                }
            }

            state = BTNodeState.FAILURE;
            return state;  // Return FAILURE if all children fail
        }
    }
}