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
    public static Action onUsePotion { get; set; }
    public static Action onEndOfInvincibility { get; set; }

    [HideInInspector] public bool isFacingRight = true;
    [HideInInspector] public bool isFacingUp = true;

    private bool isInvincible = false;

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

    private void OnEnable() 
    {
        onEndOfInvincibility += EndOfInvincibility;
    }

    private void OnDisable() 
    {
        onEndOfInvincibility -= EndOfInvincibility;
    }

    void EndOfInvincibility()
    {
        isInvincible = false;
    }

    void UpdatePlayerUi()
    {
        DisplayHealth.onUpdateHpMax?.Invoke(stats);
    }

    public override void TakeDamage(int damage, Vector3 originPosOfDamage, float power)
    {
        if (isInvincible) return;

        isInvincible = true; // to only get hit once

        base.TakeDamage(damage, originPosOfDamage, power); // to make him take damage et do the defeat if he die

        DisplayHealth.onUpdate?.Invoke(); // update the Ui
        PlayerState.onInvincible?.Invoke();

        PlayerState.onKnockBack?.Invoke(originPosOfDamage, power);
    }
    

    public override void Healing(int heal)
    {
        base.Healing(heal);
        DisplayHealth.onUpdate?.Invoke();
    }

    public override void Defeat()
    {
        Debug.Log("Player is dead");
        PlayerInputScript.onIsPlayerDead?.Invoke(true);
    }

}