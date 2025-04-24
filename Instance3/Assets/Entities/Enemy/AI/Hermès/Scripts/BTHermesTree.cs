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
        [HideInInspector] public Rigidbody2D rb;

        [Header("Movement")]
        public Transform raycast;
        public Transform targetTransform;

        [Header("Dash")]
        [Range(0.1f, 5f)] public float dashSpeedMultiplier = 2f;
        [HideInInspector] public float dashSpeed = 1f;
        
        [Tooltip("dashChance in Percent")] 
        [Range(0.1f, 100f)]public float dashChance = 50f;
        public float dashDuration = 0.3f;
        public float chargeDelay = 1f;
        public float coolDownEnterDash = 1f;

        [Header("Bool")]
        public bool waited = false;
        public bool actionStarted = false;
        public bool charged = false;
        public bool hasJumped = false;

        [Header("Jump")]
        [Range(0.1f, 5f)] public float jumpSpeedMultiplier = 2f;
        [HideInInspector] public float jumpSpeed = 1f;
        [HideInInspector] public Vector3 target;
        
        [Range(1f, 10f)] public float diveSpeedMultiplier = 3f;
        [HideInInspector] public float diveSpeed = 1f;
        
        public float timeInAir = 1f;
        [Range(0, 100)] public float percentToTargetTime = 87.5f;
        [HideInInspector] public float targetTime;
        [HideInInspector] public Vector2 lastDashDirection = Vector2.right;

        [Header("FX")]
        [HideInInspector] public ArrowForHermesFX arrowForHermesFX;
        [HideInInspector] public HermesDashFX hermesDashFX;
        [HideInInspector] public HermesDiveFX hermesDiveFX;
        [HideInInspector] public string sfxDashName = "HermesDash";
        [HideInInspector] public string sfxDiveName = "HermesDive";
        [HideInInspector] public EnemyDetectPlayerFX fxDetectPlayer;
        public GameObject dashFeedback;
        [HideInInspector] public Vector3 initialScale;

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
            initialScale = transform.localScale;
            dashSpeed = stats.speed * dashSpeedMultiplier;
            diveSpeed = stats.speed * diveSpeedMultiplier;
            jumpSpeed = stats.speed * jumpSpeedMultiplier;
            
            arrowForHermesFX = GetComponentInChildren<ArrowForHermesFX>();
            hermesDashFX = GetComponentInChildren<HermesDashFX>();
            hermesDiveFX = GetComponentInChildren<HermesDiveFX>(); 
            rb = GetComponent<Rigidbody2D>();
            fxDetectPlayer = GetComponentInChildren<EnemyDetectPlayerFX>();

            targetTime = timeInAir * (percentToTargetTime / 100); // 2 * 0.875f
            
            // SFX
            sfxDashName = "HermesDash";
            sfxDiveName = "HermesDive";
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
