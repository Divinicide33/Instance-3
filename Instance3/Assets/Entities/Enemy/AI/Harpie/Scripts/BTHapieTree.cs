using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Harpie
{
    public class BTHapieTree : BTTree
    {
        [Header("Characters")]
        [HideInInspector] public Transform treeTransform;
        [HideInInspector] public Transform playerTransform;
        [HideInInspector] public Rigidbody2D rb;

        [Header("Movement")]
        [Tooltip("ChargeDelay in seconds")]
        public float chargeDelay = 0.5f;
        [HideInInspector] public float returnSpeed = 5f;
        [Range(1f, 10f)] public float diveSpeedMultiplier = 3f;
        [HideInInspector] public float diveSpeed= 1f;
        public  float coolDownTime;
        public LayerMask GroundMask;
        
        [Header("Detection")]
        public float detectionRadius = 4f;

        [Header("Attack")]
        public float attackRange = 1.5f;
        public float knockbackForce = 5f;

        [Header("Hide")]
        [HideInInspector] public float lastDiveTime = Mathf.NegativeInfinity;
        [HideInInspector] public Vector3 target;
        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public Vector3 origin;
        [HideInInspector] public Quaternion idleRotation;
        [HideInInspector] public Vector3 lastPlayerPosition;

        [Header("Bool")]
        [HideInInspector] public bool detectedPlayer = false;
        [HideInInspector] public bool charged = false;

        [Header("FX")]
        [HideInInspector] public EnemyDetectPlayerFX fxDetectPlayer;

        private void OnEnable()
        {
            GivePlayerForEnnemy.onSetPlayerTarget += SetTarget;
        }

        private void OnDisable()
        {
            GivePlayerForEnnemy.onSetPlayerTarget -= SetTarget;
        }
        
        private void SetTarget(Transform playerTransform)
        {
            this.playerTransform = playerTransform;
        }
        
        private void Init()
        {
            TryGetComponent(out stats);
            rb = GetComponent<Rigidbody2D>();
            rb.simulated = false;
            fxDetectPlayer = GetComponentInChildren<EnemyDetectPlayerFX>();
            
            returnSpeed = stats.speed;
            diveSpeed = stats.speed * diveSpeedMultiplier;

            treeTransform = transform;
            origin = treeTransform.position;
            idleRotation = treeTransform.rotation;

        }

        protected override BTNode SetupTree()
        {
            Init();

            BTNode root = new BTSelector(new List<BTNode>
            {
                 new BTSequence(new List<BTNode>
                 {
                    new BTAction_Cooldown(this),
                    new BTAction_CheckPlayerInRange(this),
                    new BTAction_Delay(this),
                    new BTAction_DiveToPlayer(this),
                 }),
                 new BTAction_PatrolHarpie(this),
            });
            return root;
        }
    }
}

