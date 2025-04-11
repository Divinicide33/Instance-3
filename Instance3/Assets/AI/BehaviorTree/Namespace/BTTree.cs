using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTTree : MonoBehaviour
    {
        private BTNode root = null; // The root node of the behavior tree
        public BTNode Root { get { return root; } } // Public property to access the root node

        // Called when the script instance is being loaded
        protected virtual void Start()
        {
            root = SetupTree(); // Setup the behavior tree and assign the root node
        }

        // Called once per frame
        protected virtual void Update()
        {
            if (root != null)
                root.Evaluate(); // Evaluate the behavior tree starting from the root node
        }

        // Abstract method to be implemented by derived classes to setup the behavior tree
        protected abstract BTNode SetupTree();
    }
}