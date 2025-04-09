using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTTree : MonoBehaviour
    {
        private BTNode _root = null; // Le n�ud racine de l'arbre de comportement

        public BTNode Root { get { return _root; } } // Propri�t� publique pour acc�der au n�ud racine

        // Appel� lorsque l'instance du script est charg�e
        protected virtual void Start()
        {
            _root = SetupTree(); // Configurer l'arbre de comportement et assigner le n�ud racine
        }

        // Appel� une fois par frame
        protected virtual void Update()
        {
            if (_root != null)
                _root.Evaluate(); // �valuer l'arbre de comportement en commen�ant par le n�ud racine
        }

        // M�thode abstraite pour configurer l'arbre de comportement
        protected abstract BTNode SetupTree();
    }
}
