using System;
using UnityEngine;

public class PlayerState : SkillModule
{
    Stats stats;
    public static Action onInvincible { get; set; }
    public static Action<int> onTakeDamage { get; set; }
    public static Action onKnockBack { get; set; }

    [SerializeField] private float invincibilityDuration = 1f;
    private float invincibilityTimer = 0;
    private bool isInvincible = false;
    [SerializeField] private float knockBackDuration = 0.5f;
    private float knockBackTimer = 0;
    private bool isKnockedBack = false;

    private void Invincible()
    {
        if (isInvincible) return;
        //Debug.Log("onInvincible is called");

        isInvincible = true;
        invincibilityTimer = 0;

    }

    private void TakeDamage(int damage)
    {
        if (isInvincible) return;
        //Debug.Log("onTakeDamage is called");

        stats.health -= damage;
        if (stats.health < 0) stats.health = 0;
        DisplayHealth.onUpdate?.Invoke();
    }

    private void KnockBack()
    {
        //Debug.Log("onKnockBack is called");

        isKnockedBack = true;
        knockBackTimer = 0;

        PlayerInputScript.onDisableInput?.Invoke();
    }

    private void OnEnable()
    {
        if (stats == null) stats = GetComponent<Stats>();
        onInvincible += Invincible;
        onTakeDamage += TakeDamage;
        onKnockBack += KnockBack;
    }

    private void OnDisable()
    {
        onInvincible -= Invincible;
        onTakeDamage -= TakeDamage;
        onKnockBack -= KnockBack;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.J)) onInvincible?.Invoke();
        // if (Input.GetKeyDown(KeyCode.K)) onTakeDamage?.Invoke(2);
        // if (Input.GetKeyDown(KeyCode.L)) onKnockBack?.Invoke();

        if (isInvincible) 
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityDuration <= invincibilityTimer)
            {
                isInvincible = false;
                //Debug.Log("End of Invincibility");
            }
        }

        if (isKnockedBack) 
        {
            knockBackTimer += Time.deltaTime;
            if (knockBackDuration <= knockBackTimer)
            {
                isKnockedBack = false;
                PlayerInputScript.onEnableInput?.Invoke();
                //Debug.Log("End of KnockBack");
            }
        }
    }

}
