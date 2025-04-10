using UnityEngine;
using BehaviorTree;
using System.Collections.Generic;

public class BTBoarTree : BTTree
{
    [Header("Références aux objets")]
    [SerializeField] public GameObject boar;
    [SerializeField] public GameObject player;

    [Header("Paramètres de mouvement")]
    [SerializeField] public float moveSpeed = 2f;

    [Header("Paramètres de charge")]
    [SerializeField] public float chargeDelay = 1f;
    [SerializeField] public float dashSpeed = 10f;
    [SerializeField] public float dashDuration = 0.3f;

    [Header("Paramètres de détection")]
    [SerializeField] public Transform fovOrigin;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public float detectionRadius = 10f;
    [SerializeField] public float fovAngle = 60f;

    [Header("Dégâts et Knockback")]
    [SerializeField] public float damage = 10f;
    [SerializeField] public float knockbackForce = 5f;

    [SerializeField]
    public LayerMask obstacleLayer;

    public Vector3 target = Vector3.zero;

    protected override BTNode SetupTree()
    {
        BTNode root = new BTSelector(new List<BTNode>
        {
            new BTSequence(new List<BTNode>
            {
                new BTAction_CheckForTarget(this),
                new BTAction_Charge(this, boar.transform),
            }),
            new BTAction_Patrol(this),
        });

        return root;
    }
}
