using UnityEngine;

namespace BehaviorTree
{
    public class BTAction_Attack : BTNode
    {
        public GameObject _attacker;
        public GameObject _target;
        public float _damage;

        public BTAction_Attack(BTBoarTree btParent)
        {
            _attacker = btParent.gameObject;
            _target = btParent.gameObject;
            _damage = btParent.damage;
        }

        public override BTNodeState Evaluate()
        {
            if (_target != null)
            {
                //Health targetHealth = _target.GetComponent<Health>();
                //if (targetHealth != null)
                //{
                //    targetHealth.TakeDamage(_damage);  // Applique les dégâts au joueur
                //    return BTNodeState.SUCCESS;
                //}
            }

            return BTNodeState.FAILURE;
        }
    }
}
