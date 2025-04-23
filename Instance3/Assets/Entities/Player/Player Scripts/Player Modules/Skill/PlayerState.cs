using System;
using UnityEngine;

public class PlayerState : SkillModule
{
    public static Action onInvincible { get; set; }
    public static Action onEndOfInvincibility { get; set; }
    public static Action<Vector3, float> onKnockBack { get; set; }


    [Tooltip("invincibilityDuration in seconds")]
    [SerializeField] private float invincibilityDuration = 1f;

    [Tooltip("knockBackDuration in seconds")]
    [SerializeField] private float knockBackDuration = 0.5f;
    
    private Stats stats;
    private Rigidbody2D rb;

    [Header("Invincibility variable")]
    private float invincibilityTimer = 0;
    private bool isInvincible = false;

    [Header("KnockBack variable")]
    private float knockBackTimer = 0;
    private bool isKnockedBack = false;

    private void Invincible()
    {
        if (isInvincible) 
            return;

        isInvincible = true;
        invincibilityTimer = 0;
    }
    
    private void EndOfInvincibility()
    {
        isInvincible = false;
        PlayerController.onEndOfInvincibility?.Invoke();
    }

    private void KnockBack(Vector3 originPosOfDamage, float power)
    {

        isKnockedBack = true;
        knockBackTimer = 0;

        Vector2 direction =  transform.position - originPosOfDamage;

        if (direction.x > 0)
            direction.x = 1;

        else if (direction.x < 0)
            direction.x = -1;

        direction.y = 1;

        PlayerInputScript.onDisableInput?.Invoke();
        
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * power, ForceMode2D.Impulse);

    }

    private void OnEnable()
    {
        if (stats == null)
            TryGetComponent(out stats);
        
        if (rb == null) 
            TryGetComponent(out rb);

        onInvincible += Invincible;
        onEndOfInvincibility += EndOfInvincibility;
        onKnockBack += KnockBack;
    }

    private void OnDisable()
    {
        onInvincible -= Invincible;
        onEndOfInvincibility -= EndOfInvincibility;
        onKnockBack -= KnockBack;
    }

    void Update()
    {

        if (isInvincible) 
        {
            invincibilityTimer += Time.deltaTime;

            if (invincibilityDuration <= invincibilityTimer)
            {
                EndOfInvincibility();
            }
        }

        if (isKnockedBack) 
        {
            knockBackTimer += Time.deltaTime;

            if (knockBackDuration <= knockBackTimer)
            {
                isKnockedBack = false;
                rb.linearVelocityX = 0;
                PlayerInputScript.onEnableInput?.Invoke();
            }
        }
    }

}
