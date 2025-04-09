using UnityEngine;

namespace BehaviorTree
{
    public class BTAction_Charge : BTNode
    {
        private Transform _boar;
        private GameObject _target;

        private float _chargeDelay;
        private float _dashSpeed;
        private float _dashDuration;
        private float _timer = 0f;
        private bool _charging = false;
        private Vector2 _dashDirection;
        private float _dashTimer = 0f;

        private Transform _fovOrigin;
        private LayerMask _playerLayer;

        // Raycast pour vérifier si un obstacle bloque la charge
        [SerializeField] private float raycastDistance = 0.5f;

        public BTAction_Charge(Transform boar, float chargeDelay, float dashSpeed, float dashDuration, Transform fovOrigin, LayerMask playerLayer)
        {
            _boar = boar;
            _target = boar.gameObject;
            _chargeDelay = chargeDelay;
            _dashSpeed = dashSpeed;
            _dashDuration = dashDuration;
            _fovOrigin = fovOrigin;
            _playerLayer = playerLayer;
        }

        public override BTNodeState Evaluate()
        {
            if (!_charging)
            {
                _timer += Time.deltaTime;
                if (_timer >= _chargeDelay)
                {
                    // Calcul de la direction de charge vers le joueur
                    Vector2 directionToTarget = (_target.transform.position - _boar.position).normalized;
                    _dashDirection = new Vector2(Mathf.Sign(directionToTarget.x), 0f); // Dash horizontal uniquement

                    // Check if the target is in the FOV and within range
                    RaycastHit2D hit = Physics2D.Raycast(_fovOrigin.position, _dashDirection, raycastDistance, _playerLayer);
                    if (hit.collider != null && hit.collider.gameObject == _target)
                    {
                        _charging = true;
                        Debug.Log("Charge started");  // Ajout du log de début de charge
                    }
                    else
                    {
                        Debug.Log("Charge failed: Target not in FOV or out of range.");
                    }
                }
                else
                {
                    return BTNodeState.RUNNING;
                }
            }
            else
            {
                _dashTimer += Time.deltaTime;

                // Appliquer le dash uniquement sur l'axe X, Y reste inchangé
                _boar.position += (Vector3)(_dashDirection * _dashSpeed * Time.deltaTime);

                // Log de déplacement pendant le dash
                Debug.Log($"Dash moving, current position: {_boar.position}");

                // Si le dash est terminé, réinitialiser les variables et retourner SUCCESS
                if (_dashTimer >= _dashDuration)
                {
                    _timer = 0f;
                    _dashTimer = 0f;
                    _charging = false;
                    Debug.Log("Charge completed");  // Ajout du log à la fin de la charge
                    return BTNodeState.SUCCESS;
                }

                return BTNodeState.RUNNING;
            }

            return BTNodeState.RUNNING;
        }

        // Fonction pour dessiner les Gizmos dans l'éditeur
        public void DrawGizmos()
        {
            if (_fovOrigin != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(_fovOrigin.position, _fovOrigin.position + (Vector3)(_dashDirection * raycastDistance));
            }
        }
    }
}
