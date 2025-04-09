using UnityEngine;
using BehaviorTree;

public class BTAction_CheckForTarget : BTNode
{
    private Transform _entityTransform;
    private GameObject _target;
    private float _detectionRange = 6f; // Portée de détection (rayon du cercle)
    private float _fovAngle = 60f; // Angle du champ de vision (FOV) - 60 degrés pour un cône de vision

    public BTAction_CheckForTarget(Transform entity, GameObject target)
    {
        _entityTransform = entity;
        _target = target;
    }

    public override BTNodeState Evaluate()
    {
        // 1. Calculer la direction de vision de l'entité (ici, la direction dans laquelle elle regarde/mouvement)
        Vector2 forwardDirection = _entityTransform.right;  // On suppose que l'entité regarde dans la direction X (ou vers l'avant)

        // 2. Calculer la direction du joueur par rapport à l'entité
        Vector2 targetDirection = (Vector2)_target.transform.position - (Vector2)_entityTransform.position;

        // 3. Calculer l'angle entre la direction de l'entité et celle du joueur
        float angleBetween = Vector2.Angle(forwardDirection, targetDirection);

        // Log pour vérifier les calculs
        Debug.Log($"Forward Direction: {forwardDirection}, Target Direction: {targetDirection}");
        Debug.Log($"Angle Between: {angleBetween}");

        // 4. Vérifier si l'angle est dans le champ de vision
        if (angleBetween <= _fovAngle / 2)
        {
            // 5. Vérifier la distance au joueur pour s'assurer qu'il est dans la portée du FOV
            float distance = Vector2.Distance(_entityTransform.position, _target.transform.position);
            Debug.Log($"Distance to Target: {distance}");

            if (distance <= _detectionRange)
            {
                // Si dans le FOV et à portée, détecter le joueur
                Debug.Log("Player detected!");
                return BTNodeState.SUCCESS;
            }
        }

        // Si le joueur est hors du FOV ou hors de portée, ne pas le détecter
        return BTNodeState.FAILURE;
    }
}
