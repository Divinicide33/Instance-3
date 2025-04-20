using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private bool isAttacking = false;
    private bool isDashing = false;

    public static Action<bool> onIsAttacking { get; set; }
    public static Action onStopAction { get; set; }

    [Header("FX")]
    private AttackFX attackFX;
    private string sfxName = "PlayerAttack";
    DashFX dashFx;
    private void Awake()
    {
        attackFX = GetComponentInChildren<AttackFX>();
        dashFx = GetComponentInChildren<DashFX>();
    }
    private void OnEnable() 
    {
        PlayerController.onAttack += Attack;
        PlayerDash.onSetIsDashing += SetIsDashing;
        onStopAction += EndOfTheAttack;
    }
    private void OnDisable() 
    {
        PlayerController.onAttack -= Attack;
        PlayerDash.onSetIsDashing -= SetIsDashing;
        onStopAction -= EndOfTheAttack;
    }

    private void SetIsDashing(bool value)
    {
        isDashing = value;
    }

    void Attack()
    {
        if (isAttacking) return;
        if (isDashing) return;

        isAttacking = true;
        onIsAttacking?.Invoke(isAttacking);

        PlayerSword.onSword?.Invoke();

        attackFX?.ShowSFX(sfxName);
    }

    public void EndOfTheAttack()
    {
        isAttacking = false;
        onIsAttacking?.Invoke(isAttacking);
        PlayerAnimator.onSetIsAttacking?.Invoke(isAttacking);
    }

}
