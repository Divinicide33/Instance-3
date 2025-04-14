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
        public Action action = Action.None;

        [Header("Characters")]
        public Transform tree;
        public Transform player;

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
        public float knockBackForce = 5f;

        [Header("Bool")]
        public bool waited = false;
        public bool reachedTarget = false;
        public bool actionStarted = false;
        public bool charged = false;
        public bool hasJumped = false;

        [Header("Jump")]
        public float jumpHeight;
        public float jumpDuration;
        public float jumpSpeed;
        [HideInInspector] public Vector3 target;
        public float targetTime;
        public float diveSpeed = 15f;
        public float timeInAir = 1f;

        [Header("Layer")]
        [SerializeField] public LayerMask playerLayer;
        [SerializeField] public LayerMask obstacleLayer;

        [HideInInspector] public Vector2 lastDashDirection = Vector2.right;
        public void FlipDirection(ref Vector2 direction)
        {
            direction *= -1;

            Vector3 scale = tree.localScale;
            scale.x *= -1;
            tree.localScale = scale;
        }

        protected override BTNode SetupTree()
        {
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
