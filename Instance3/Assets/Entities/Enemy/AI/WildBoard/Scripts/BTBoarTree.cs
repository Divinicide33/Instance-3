using UnityEngine;
using BehaviorTree;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

namespace AI.WildBoard
{
    public class BTBoarTree : BTTree
    {
        [Header("Parametres de mouvement")]
        [HideInInspector] public float moveSpeed = 2f;
        public GameObject dashFeedback;

        [Header("Parametres de charge")]
        [Tooltip("Delay in seconds.")] 
        public float chargeDelay = 1f;
        public bool dashStarted = false;
        
        [Range(0.1f, 5f)] public float dashSpeedMultiplier = 2f;
        [HideInInspector] public float dashSpeed = 10f;
        [Tooltip("Duration in seconds.")] 
        public float dashDuration = 0.3f;

        [Header("Parametres de detection")]
        public Transform fovOrigin;

        public LayerMask playerLayer;
        public float detectionRadius = 10f;
        public float fovAngle = 60f;
        public float DurationEnterDash = 2f;

        [Header("Degets et Knockback")]
        public LayerMask obstacleLayer;
        public Vector2 lastDashDirection = Vector2.right;
        public Vector2 direction = Vector2.right;

        [Header("FX")]
        [HideInInspector] public EnemyDetectPlayerFX fxDetectPlayer;


        private void Init()
        {
            moveSpeed = stats.speed;
            dashSpeed = stat.speed * dashSpeedMultiplier;
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

        public void FlipDirection()
        {
            direction *= -1;
            Vector3 currentEuler = transform.eulerAngles;
            transform.eulerAngles = new Vector3(currentEuler.x, currentEuler.y + 180f, currentEuler.z);
        }
    }
}