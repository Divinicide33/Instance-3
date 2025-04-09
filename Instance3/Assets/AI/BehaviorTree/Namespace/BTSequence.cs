using System.Collections.Generic;

namespace BehaviorTree
{
    public class BTSequence : BTNode
    {
        // Constructor for a sequence node without children
        public BTSequence() : base() { }

        // Constructor for a sequence node with children
        public BTSequence(List<BTNode> children) : base(children) { }

        // Evaluate the sequence node
        public override BTNodeState Evaluate()
        {
            bool anyChildIsRunning = false;  // Flag to track if any child is running

            // Iterate through each child node
            foreach (BTNode BTNode in _children)
            {
                // Exit the loop if any child is running
                if (anyChildIsRunning)
                {
                    break;
                }

                // Evaluate the current child node
                switch (BTNode.Evaluate())
                {
                    case BTNodeState.FAILURE:
                        _state = BTNodeState.FAILURE;
                        return _state;  // Return FAILURE if any child fails

                    case BTNodeState.SUCCESS:
                        continue;  // Continue to the next child if the current one succeeds

                    case BTNodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;  // Set the flag and continue to the next child if the current one is running

                    default:
                        _state = BTNodeState.SUCCESS;
                        return _state;  // Return SUCCESS by default
                }
            }

            // Set the state based on whether any child is running
            _state = anyChildIsRunning ? BTNodeState.RUNNING : BTNodeState.SUCCESS;
            return _state;
        }
    }
}
