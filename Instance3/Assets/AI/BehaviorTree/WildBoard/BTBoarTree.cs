using UnityEngine;
using System.Collections.Generic;
using BehaviorTree;

public class BTBoarTree : BTTree
{
    [SerializeField] private GameObject boar;
    [SerializeField] private GameObject player;
    [SerializeField] private float moveSpeed = 2f;

    // Charge config exposée dans l'inspecteur
    [SerializeField] private float chargeDelay = 1f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.3f;

    // Ajout du FOV
    [SerializeField] private Transform fovOrigin;
    [SerializeField] private LayerMask playerLayer;

    private BTAction_Charge _chargeNode; // Pour dessiner le Gizmo
    private BTAction_CheckForTarget _checkForTargetNode;

    protected override BTNode SetupTree()
    {
        // Création des nœuds
        _checkForTargetNode = new BTAction_CheckForTarget(boar.transform, player);  // Détecter dans un rayon de 6 unités
        _chargeNode = new BTAction_Charge(boar.transform,chargeDelay, dashSpeed, dashDuration ,fovOrigin , playerLayer);

        var engage = new BTSequence(new List<BTNode> { _checkForTargetNode, _chargeNode });
        var patrol = new BTAction_Patrol(boar.transform, moveSpeed, player, fovOrigin);

        return new BTSelector(new List<BTNode> { engage, patrol });
    }

}
