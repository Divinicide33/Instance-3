using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

public class HermèsBehaviorTree : BTTree
{
    public enum HermesAction
    {
        None,
        Dash,
        Jump,
    }

    public HermesAction Action = HermèsBehaviorTree.HermesAction.None;

    public Transform Tree;
    public Transform player;

    [Header("Déplacement")]
    public float speed = 4f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.3f;
    public float DashChance = 10;
    public float chargeDelay = 1f;
    public float CoolDownEnterDash = 1f;

    public bool actionStarted = false;

    [Header("Jump")]
    public float _jumpHeight;
    public float _jumpDuration;

    [SerializeField] public float knockbackForce = 5f;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public LayerMask obstacleLayer;
    [HideInInspector] public Vector2 lastDashDirection = Vector2.right;

    protected override BTNode SetupTree()
    {
        BTNode root = new BTSelector(new List<BTNode>
        {
            new BTSequence(new List<BTNode>
            {
                new BTAction_Cooldown(this),
                new BTAction_Delay(this),
                new BTSelector(new List<BTNode>
                {
                    new BTSequence(new List<BTNode>
                    {
                        new BTAction_DashChance(this),
                        new BTAction_DashHermes(this)
                    }),

                    new BTAction_JumpCharge(this)
                })
            }),

            new BTAction_PatrolHermes(this),
        });

        return root;
    }
}
