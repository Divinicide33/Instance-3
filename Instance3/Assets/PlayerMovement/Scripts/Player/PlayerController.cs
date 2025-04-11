using System;
using UnityEngine;

[RequireComponent(typeof(Stats))]
public class PlayerController : Entity
{
    private Stats stats;
    private PlayerMove move;
    private PlayerJump jump;
    private PlayerAttack attack;
    private PlayerDash dash;
    private PlayerGlide glide;

    public static Action<Vector2> onMove { get; set; }
    public static Action<bool> onJump { get; set; }
    public static Action onAttack { get; set; }
    public static Action onDash { get; set; }
    public static Action<bool> onGlide { get; set; }

    [HideInInspector] public bool isFacingRight = true;

    void Start()
    {
        stats = GetComponent<Stats>();
        move = GetComponent<PlayerMove>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<PlayerAttack>();
        dash = GetComponent<PlayerDash>();
        glide = GetComponent<PlayerGlide>();

        UpdatePlayerUi();

    }

    void UpdatePlayerUi()
    {
        DisplayHealth.onUpdateHpMax?.Invoke(stats);
    }
}
