using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private Stats stats;
    private Vector2 direction;
    private bool canMove = true;
    private bool isDashing = false;
    public static Action<bool> onSetMove { get; set; }
    public static Action onResetVelocity { get; set; }

    private void Awake() 
    {
        stats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        PlayerController.onMove += Move;
        onSetMove += SetCanMove;
        PlayerDash.onSetIsDashing += SetIsDashing;
        onResetVelocity += ResetVelocity;
    }
    
    void OnDisable()
    {
        PlayerController.onMove -= Move;
        onSetMove -= SetCanMove;
        PlayerDash.onSetIsDashing -= SetIsDashing;
        onResetVelocity -= ResetVelocity;
    }

    private void ResetVelocity()
    {
        canMove = false; 
        rb.linearVelocity = Vector2.zero;
    }

    private void SetCanMove(bool value)
    {
        canMove = value; 
        rb.linearVelocityX = 0;
    }

    private void SetIsDashing(bool value)
    {
        isDashing = value;
        rb.linearVelocityX = 0;
    }

    private void Update() 
    {
        if (canMove && !isDashing)
        {
            rb.linearVelocityX = direction.x * stats.speed;
        }
    }

    public void Move(Vector2 inputDirection)
    {
        direction = inputDirection;
    }
}
