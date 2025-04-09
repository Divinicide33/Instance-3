using UnityEngine;
using BehaviorTree;

public class BTAction_Patrol : BTNode
{
    private Transform _boar;                // Référence au sanglier
    private float _moveSpeed;               // Vitesse de déplacement
    private GameObject _player;             // Référence au joueur
    private Vector2 _direction;             // Direction de déplacement

    private float _detectionDistance = 0.75f;  // Distance des raycasts
    private Transform _fovOrigin;           // Origine du FOV (utilisée pour détecter les obstacles)

    private LayerMask _platformLayerMask;   // Layer Mask pour la plateforme

    public BTAction_Patrol(Transform boar, float moveSpeed, GameObject player, Transform fovOrigin)
    {
        _boar = boar;
        _moveSpeed = moveSpeed;
        _player = player;
        _direction = Vector2.right;  // Direction initiale vers la droite
        _fovOrigin = fovOrigin;
        _platformLayerMask = LayerMask.GetMask("Platform");  // Assurez-vous que vous avez un Layer "Platform"
    }

    public override BTNodeState Evaluate()
    {
        // Raycast pour vérifier s'il y a un obstacle devant le sanglier (mur)
        RaycastHit2D hitObstacle = Physics2D.Raycast(_fovOrigin.position, _direction, _detectionDistance, _platformLayerMask);

        // Si une collision avec une plateforme (mur) est détectée
        if (hitObstacle.collider != null)
        {
            // Inverser la direction (tourner de 180° sur l'axe Y)
            _direction *= -1;
            Vector3 currentEuler = _boar.eulerAngles;
            _boar.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
        }

        // Raycast pour vérifier si le sanglier est au sol
        RaycastHit2D hitGround = Physics2D.Raycast(_fovOrigin.position, Vector2.down, _detectionDistance);

        // Si le sanglier n'est pas au sol, changer de direction
        if (hitGround.collider == null)
        {
            // Inverser la direction (tourner de 180° sur l'axe Y)
            _direction *= -1;
            Vector3 currentEuler = _boar.eulerAngles;
            _boar.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
        }

        // Déplacer le sanglier dans la direction actuelle
        _boar.position += (Vector3)(_direction.normalized * _moveSpeed * Time.deltaTime);

        // Dessiner les raycasts pour visualiser dans l'éditeur
        Debug.DrawRay(_fovOrigin.position, _direction * _detectionDistance, Color.red); // Raycast pour les plateformes
        Debug.DrawRay(_boar.position, Vector2.down * _detectionDistance, Color.green);  // Raycast pour le sol

        return BTNodeState.RUNNING;  // Le comportement continue tant que le sanglier patrouille
    }
}
