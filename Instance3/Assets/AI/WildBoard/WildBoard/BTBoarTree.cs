using UnityEngine;
using BehaviorTree;
using System.Collections.Generic;

namespace AI.WildBoard
{
    public class BTBoarTree : BTTree
    {
        [Header("Param�tres de mouvement")]
        public float moveSpeed = 2f;

        [Header("Param�tres de charge")]
        public float chargeDelay = 1f;
        public bool dashStarted = false;
        public float dashSpeed = 10f;
        public float dashDuration = 0.3f;

        [Header("Param�tres de d�tection")]
        public Transform fovOrigin;

        public LayerMask playerLayer;
        public float detectionRadius = 10f;
        public float fovAngle = 60f;
        public float DurationEnterDash = 2f;

        [Header("D�g�ts et Knockback")]
        public float damage = 10f;

        public float knockbackForce = 5f;

        public LayerMask obstacleLayer;

        public Vector2 lastDashDirection = Vector2.right;

        [Header("FX")]
        [HideInInspector] public EnemyDetectPlayerFX fxDetectPlayer;

        private void Init()
        {
            moveSpeed = stats.speed;
            dashSpeed = stat.speed * 2;
            fxDetectPlayer = GetComponentInChildren<EnemyDetectPlayerFX>();
        }

        protected override BTNode SetupTree()
        {
            Init();

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