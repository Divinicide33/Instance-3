using System.Collections.Generic;

namespace BehaviorTree
{
    // L'�tat d'un n�ud
    public enum BTNodeState
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    // Classe de base pour les n�uds du comportement
    public abstract class BTNode
    {
        // Liste des enfants du n�ud
        protected List<BTNode> _children;

        // L'�tat actuel du n�ud
        protected BTNodeState _state;

        // Constructeur sans enfants
        public BTNode()
        {
            _children = new List<BTNode>();
            _state = BTNodeState.RUNNING; // Par d�faut, le n�ud est en cours d'ex�cution
        }

        // Constructeur avec des enfants
        public BTNode(List<BTNode> children)
        {
            _children = children;
            _state = BTNodeState.RUNNING;
        }

        // M�thode abstraite pour �valuer le n�ud
        public abstract BTNodeState Evaluate();
    }
}
