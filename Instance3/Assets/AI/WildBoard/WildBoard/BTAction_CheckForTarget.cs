using UnityEngine;
using BehaviorTree;
using static UnityEngine.GraphicsBuffer;

namespace AI.WildBoard
{
    public class BTAction_CheckForTarget : BTNode
    {
        private BTBoarTree _bt;

        public BTAction_CheckForTarget(BTBoarTree bt)
        {
            _bt = bt;
        }

        public override BTNodeState Evaluate()
        {
            /*Vector2 directionToPlayer = _bt.player.transform.position - _bt.fovOrigin.position;
            float distance = directionToPlayer.magnitude;
            if (distance <= _bt.detectionRadius)
            {
                float angle = Vector2.Angle(_bt.fovOrigin.right, directionToPlayer.normalized);
                if (angle < _bt.fovAngle / 2f)
                {

                    Vector3 directionDebug = _bt.player.transform.position - _bt.boar.transform.position;
                    Debug.DrawRay(_bt.boar.transform.position, directionDebug, Color.yellow);

                    Vector2 direction = (_bt.player.transform.position - _bt.boar.transform.position).normalized;
                    //_bt.boar.transform.position += (Vector3)(direction * _bt.dashSpeed * Time.deltaTime);
                    Vector2 origin = _bt.boar.transform.position;
                    Vector2 target = _bt.player.transform.position;
                    float distanceDash = Vector2.Distance(origin, target);

                    RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);

                    if (hits.Length > 0)
                    {
                        RaycastHit2D firstHit = hits[0];

                        if (firstHit.collider.gameObject.layer != _bt.playerLayer)
                        {
                            Debug.Log("NotPlayer");
                            _bt.target = Vector3.zero;
                            _state = BTNodeState.FAILURE;
                            return _state;
                        }
                        else
                        {
                            Debug.Log("Succes");
                            //_chargeAction.SetChargeTarget(_bt.player.transform.position);
                            _bt.target = _bt.player.transform.position;
                            _state = BTNodeState.SUCCESS;
                            return _state;
                        }
                    }
                }
            }*/

            state = BTNodeState.FAILURE;
            return state;
        }
    }
}