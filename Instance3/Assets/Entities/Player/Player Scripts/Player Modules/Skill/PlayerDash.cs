using System;
using UnityEngine;

public class PlayerDash : SkillModule
{
    private Rigidbody2D rb;
    private PlayerController player;
    private PlayerMove playerMove;

    [Header("Dash Variable")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float timerDash;
    private float timerCooldown;
    private bool canDash = true;
    private bool isDashing = false;
    private int direction;
    private float height;
    private bool isGrounded = false;

    public static Action onStopDash { get; set; }
    public static Action<bool> onSetIsDashing { get; set; }

    private bool isAttacking = false;
    
    [Header("FX")]
    private DashFX dashFx;
    private string sfxName = "PlayerDash";

    void Awake()
    {
        player = GetComponent<PlayerController>();
        playerMove = GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody2D>();
        dashFx = GetComponentInChildren<DashFX>();
    }

    void OnEnable()
    {
        PlayerController.onDash += Dash;
        PlayerAttack.onIsAttacking += SetIsAttacking;
        GroundCheck.onGrounded += ChangeBool;
        onStopDash += StopDash;
    }
    void OnDisable()
    {
        PlayerController.onDash -= Dash;
        PlayerAttack.onIsAttacking -= SetIsAttacking;
        GroundCheck.onGrounded -= ChangeBool;
        onStopDash -= StopDash;
    }
    
    private void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }
    void Update() 
    {
        if (!canDash) CheckIfCanDash();
        else timerCooldown = 0;

        if (isDashing) Dashing();
        else timerDash = 0;
    }

    private void ChangeBool(bool value)
    {
        isGrounded = value;
    }

    public void Dash()
    {
        if (!isDashing && !isAttacking && canDash)
        {
            onSetIsDashing?.Invoke(true);
            height = transform.position.y;
            rb.linearVelocityY = 0;

            if (player.isFacingRight) direction = 1;
            else direction = -1;

            canDash = false;
            isDashing = true;

            dashFx?.ShowVFX();
            dashFx?.ShowSFX(sfxName);
        }
    }

    void Dashing()
    {
        timerDash += Time.deltaTime;

        transform.position = new Vector3(transform.position.x, height, transform.position.z);
        rb.linearVelocityX = direction * dashForce;
        
        if (timerDash >= dashDuration)
        {
            StopDash();
        }

    }
    void CheckIfCanDash()
    {
        timerCooldown += Time.deltaTime;
        if (timerCooldown >= dashCooldown && isGrounded) canDash = true;
    }

    public void StopDash()
    {
        isDashing = false;
        onSetIsDashing?.Invoke(false);
        rb.linearVelocityX = 0;
    }

}
