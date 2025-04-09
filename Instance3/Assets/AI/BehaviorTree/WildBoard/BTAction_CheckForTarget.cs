using UnityEngine;
using BehaviorTree;

public class BTAction_CheckForTarget : BTNode
{
    private Transform _entityTransform;
    private GameObject _target;
    private float _detectionRange = 6f; // Port�e de d�tection (rayon du cercle)
    private float _fovAngle = 60f; // Angle du champ de vision (FOV) - 60 degr�s pour un c�ne de vision

    public BTAction_CheckForTarget(Transform entity, GameObject target)
    {
        _entityTransform = entity;
        _target = target;
    }

    public override BTNodeState Evaluate()
    {
        // 1. Calculer la direction de vision de l'entit� (ici, la direction dans laquelle elle regarde/mouvement)
        Vector2 forwardDirection = _entityTransform.right;  // On suppose que l'entit� regarde dans la direction X (ou vers l'avant)

        // 2. Calculer la direction du joueur par rapport � l'entit�
        Vector2 targetDirection = (Vector2)_target.transform.position - (Vector2)_entityTransform.position;

        // 3. Calculer l'angle entre la direction de l'entit� et celle du joueur
        float angleBetween = Vector2.Angle(forwardDirection, targetDirection);

        // Log pour v�rifier les calculs
        Debug.Log($"Forward Direction: {forwardDirection}, Target Direction: {targetDirection}");
        Debug.Log($"Angle Between: {angleBetween}");

        // 4. V�rifier si l'angle est dans le champ de vision
        if (angleBetween <= _fovAngle / 2)
        {
            // 5. V�rifier la distance au joueur pour s'assurer qu'il est dans la port�e du FOV
            float distance = Vector2.Distance(_entityTransform.position, _target.transform.position);
            Debug.Log($"Distance to Target: {distance}");

            if (distance <= _detectionRange)
            {
                // Si dans le FOV et � port�e, d�tecter le joueur
                Debug.Log("Player detected!");
                return BTNodeState.SUCCESS;
            }
        }

        // Si le joueur est hors du FOV ou hors de port�e, ne pas le d�tecter
        return BTNodeState.FAILURE;
    }
}
