using UnityEngine;

namespace BehaviorTree
{
    public class BTAction_Attack : BTNode
    {
        private GameObject _attacker;
        private GameObject _target;
        private int _damage;

        public BTAction_Attack(GameObject attacker, GameObject target, int damage)
        {
            _attacker = attacker;
            _target = target;
            _damage = damage;
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
