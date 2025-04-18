using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Harpie
{
    public class BTHapieTree : BTTree
    {
        [Header("Characters")]
        public Transform tree;
        public Transform player;

        [Header("Movement")]
        public float returnSpeed = 5f;
        public float diveSpeed = 15f;
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
        public bool detectedPlayer = false;

        [Header("FX")]
        [HideInInspector] public EnemyDetectPlayerFX fxDetectPlayer;
        
        private void Init()
        {
            tree.GetComponent<Rigidbody2D>().simulated = false;
            origin = tree.position;
            idleRotation = tree.rotation;
            fxDetectPlayer = GetComponentInChildren<EnemyDetectPlayerFX>();
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

