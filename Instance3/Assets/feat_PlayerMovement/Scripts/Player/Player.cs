using System;
using UnityEngine;

public class Player : Entity
{
    private Stats stats;
    private PlayerMove move;
    private PlayerJump jump;
    private PlayerAttack attack;
    private PlayerDash dash;
    private PlayerGlide glide;

    [HideInInspector] public static Action<Vector2> onMove;
    [HideInInspector] public static Action<bool> onJump;
    [HideInInspector] public static Action onAttack;
    [HideInInspector] public static Action onDash;
    [HideInInspector] public static Action<bool> onGlide;

    [HideInInspector] public bool isFacingRight = true;
    void Start()
    {
        stats = GetComponent<Stats>();
        move = GetComponent<PlayerMove>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<PlayerAttack>();
        dash = GetComponent<PlayerDash>();
        glide = GetComponent<PlayerGlide>();
    }

}
