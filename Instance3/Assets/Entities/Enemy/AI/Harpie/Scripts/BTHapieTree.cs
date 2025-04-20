using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Harpie
{
    public class BTHapieTree : BTTree
    {
        [Header("Characters")]
        [HideInInspector] public Transform tree;
        [HideInInspector] public Transform player;

        [Header("Movement")]
        [HideInInspector] public float returnSpeed = 5f;
        [Range(1f, 10f)] public float diveSpeedMultiplier = 3f;
        [HideInInspector] public float diveSpeed= 1f;
        public  float coolDownTime;
        public LayerMask GroundMask;
        
        [HideInInspector] public float lastDiveTime = Mathf.NegativeInfinity;
        [HideInInspector] public Vector3 target;
        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public Vector3 origin;
        [HideInInspector] public Quaternion idleRotation;
        [HideInInspector] public Vector3 lastPlayerPosition;

        [Header("Detection")]
        public float detectionRadius = 4f;

        [Header("Attack")]
        public float attackRange = 1.5f;
        public float knockbackForce = 5f;

        [Header("Bool")]
        [HideInInspector] public bool detectedPlayer = false;

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
            player = playerTransform;
        }
        
        private void Init()
        {
            TryGetComponent(out stats);
            tree.GetComponent<Rigidbody2D>().simulated = false;
            fxDetectPlayer = GetComponentInChildren<EnemyDetectPlayerFX>();
            
            returnSpeed = stats.speed;
            diveSpeed = stats.speed * diveSpeedMultiplier;
            
            origin = tree.position;
            idleRotation = tree.rotation;
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
                    new BTAction_DiveToPlayer(this),
                 }),
                 new BTAction_PatrolHarpie(this),
            });
            return root;
        }
    }
}

