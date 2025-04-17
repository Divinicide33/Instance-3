using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Hermes
{
    public enum Action
    {
        None,
        Dash,
        Jump,
    }

    public class BTHermesTree : BTTree
    {
        [HideInInspector] public Action action = Action.None;

        [Header("Characters")]
        [HideInInspector] public Transform tree;
        [HideInInspector] public Transform player;

        [Header("Movement")]
        public float speed = 4f;
        public Transform raycast;
        public Transform targetTransform;

        [Header("Dash")]
        public float dashSpeed = 20f;
        public float dashDuration = 0.3f;
        public float dashChance = 10;
        public float chargeDelay = 1f;
        public float coolDownEnterDash = 1f;

        [Header("Bool")]
        public bool waited = false;
        public bool actionStarted = false;
        public bool charged = false;
        public bool hasJumped = false;

        [Header("Jump")]
        [HideInInspector] public Rigidbody2D rb;
        public float jumpSpeed;
        [HideInInspector] public Vector3 target;
        public float diveSpeed = 15f;
        public float timeInAir = 1f;
        [Range(0, 100)] public float percentToTargetTime = 87.5f;
        [HideInInspector] public float targetTime;

        [HideInInspector] public Vector2 lastDashDirection = Vector2.right;
        public void FlipDirection(ref Vector2 direction)
        {
            direction *= -1;

            Vector3 scale = tree.localScale;
            scale.x *= -1;
            tree.localScale = scale;
        }
        
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
            tree = transform;
            TryGetComponent<Rigidbody2D>(out rb);
            targetTime = timeInAir * (percentToTargetTime / 100); // 2 * 0.875f
        }
        
        protected override BTNode SetupTree()
        {
            Init();
            
            BTNode root = new BTSelector(new List<BTNode>
            {
                new BTSequence(new List<BTNode>//
                {
                    new BTAction_Cooldown(this),
                    new BTAction_Delay(this),
            
                    new BTSelector(new List<BTNode>
                    {
                        new BTSequence(new List<BTNode> //Sequence Dash
                        {
                            new BTAction_DashChance(this),
                            new BTAction_DashHermes(this)
                        }),

                        new BTSequence(new List<BTNode> //Sequence Jump and Dive
                        {
                            new BTAction_Jump(this),
                            new BTAction_WaitInAir(this),
                            new BTAction_DiveToPlayer(this),
                        })
                    })
                }),

                new BTAction_PatrolHermes(this),
            });

            return root;
        }
    }
}
