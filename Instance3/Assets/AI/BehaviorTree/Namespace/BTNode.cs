using System.Collections.Generic;

namespace BehaviorTree
{
    // L'état d'un nœud
    public enum BTNodeState
    {
        SUCCESS,
        FAILURE,
        RUNNING
    }

    // Classe de base pour les nœuds du comportement
    public abstract class BTNode
    {
        // Liste des enfants du nœud
        protected List<BTNode> _children;

        // L'état actuel du nœud
        protected BTNodeState _state;

        // Constructeur sans enfants
        public BTNode()
        {
            _children = new List<BTNode>();
            _state = BTNodeState.RUNNING; // Par défaut, le nœud est en cours d'exécution
        }

        // Constructeur avec des enfants
        public BTNode(List<BTNode> children)
        {
            _children = children;
            _state = BTNodeState.RUNNING;
        }

        // Méthode abstraite pour évaluer le nœud
        public abstract BTNodeState Evaluate();
    }
}
