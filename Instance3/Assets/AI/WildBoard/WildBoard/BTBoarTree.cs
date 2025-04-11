using UnityEngine;
using BehaviorTree;
using System.Collections.Generic;

namespace AI.WildBoard
{
    public class BTBoarTree : BTTree
    {
        [Header("Param�tres de mouvement")] [SerializeField]
        public float moveSpeed = 2f;

        [Header("Param�tres de charge")] [SerializeField]
        public float chargeDelay = 1f;
        public bool dashStarted = false;
        [SerializeField] public float dashSpeed = 10f;
        [SerializeField] public float dashDuration = 0.3f;

        [Header("Param�tres de d�tection")] [SerializeField]
        public Transform fovOrigin;
        
        [SerializeField] public LayerMask playerLayer;
        [SerializeField] public float detectionRadius = 10f;
        [SerializeField] public float fovAngle = 60f;
        [SerializeField] public float DurationEnterDash = 2f;

        [Header("D�g�ts et Knockback")] [SerializeField]
        public float damage = 10f;

        [SerializeField] public float knockbackForce = 5f;

        [SerializeField] public LayerMask obstacleLayer;


        public Vector2 lastDashDirection = Vector2.right;
        protected override BTNode SetupTree()
        {
            BTNode root = new BTSelector(new List<BTNode>
            {
                new BTSequence(new List<BTNode> // sequence Charge
                {
                    //new BTCondition_IsPlayerInRange(this),
                    new BTCondition_IsPlayerInFOV(this),
                    new BTAction_ChargePattern(this),
                }),
                
                new BTAction_Patrol(this),
            });

            return root;
        }
    }
}