using System.Collections.Generic;

namespace BehaviorTree
{
    // Enum to represent the state of a behavior tree node
    public enum BTNodeState
    {
        RUNNING,  // Node is currently running
        SUCCESS,  // Node completed successfully
        FAILURE   // Node failed to complete
    }

    public class BTNode
    {
        protected BTNodeState state;  // The current state of the node

        public BTNode Parent;  // Reference to the parent node
        protected List<BTNode> children = new();  // List of child nodes

        private Dictionary<string, object> dataContext = new();  // Data context for storing key-value pairs

        // Constructor for a node without children
        public BTNode()
        {
            Parent = null;
        }

        // Constructor for a node with children
        public BTNode(List<BTNode> children)
        {
            foreach (BTNode child in children)
                Attach(child);  // Attach each child node
        }

        // Attach a child node to this node
        private void Attach(BTNode BTNode)
        {
            BTNode.Parent = this;  // Set this node as the parent of the child
            children.Add(BTNode);  // Add the child to the list of children
        }

        // Evaluate the node (default implementation returns FAILURE)
        public virtual BTNodeState Evaluate() => BTNodeState.FAILURE;

        // Set data in the node's context
        public void SetData(string key, object value)
        {
            dataContext[key] = value;
        }

        // Get data from the node's context or its ancestors' contexts
        public object GetData(string key)
        {
            if (dataContext.TryGetValue(key, out object value))
                return value;

            BTNode BTNode = Parent;
            while (BTNode != null)
            {
                value = BTNode.GetData(key);
                if (value != null)
                    return value;
                BTNode = BTNode.Parent;
            }
            return null;
        }

        // Clear data from the node's context or its ancestors' contexts
        public bool ClearData(string key)
        {
            if (dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);
                return true;
            }

            BTNode BTNode = Parent;
            while (BTNode != null)
            {
                bool cleared = BTNode.ClearData(key);
                if (cleared)
                    return true;
                BTNode = BTNode.Parent;
            }
            return false;
        }
    }
}