using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTTree : MonoBehaviour
    {
        private BTNode _root = null; // Le nœud racine de l'arbre de comportement

        public BTNode Root { get { return _root; } } // Propriété publique pour accéder au nœud racine

        // Appelé lorsque l'instance du script est chargée
        protected virtual void Start()
        {
            _root = SetupTree(); // Configurer l'arbre de comportement et assigner le nœud racine
        }

        // Appelé une fois par frame
        protected virtual void Update()
        {
            if (_root != null)
                _root.Evaluate(); // Évaluer l'arbre de comportement en commençant par le nœud racine
        }

        // Méthode abstraite pour configurer l'arbre de comportement
        protected abstract BTNode SetupTree();
    }
}
