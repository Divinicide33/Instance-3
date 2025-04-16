using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private int nbJump;
    private int nbJumpMax = 1;
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;
    private bool isGrounded = false;

    public static Action<bool> onChangeJump { get; set; }
    private void Awake() 
    {
        nbJump = nbJumpMax;
        rb = GetComponent<Rigidbody2D>();

    }

    private void ChangeJump(bool value)
    {
        if (value) nbJumpMax = 2;
        else nbJumpMax = 1;

        if (isGrounded) nbJump = nbJumpMax;
    }

    private void ResetJump(bool value)
    {
        isGrounded = value;
        if (isGrounded) nbJump = nbJumpMax;
        else if (!isGrounded && nbJump == nbJumpMax) nbJump--;
    }

    void OnEnable()
    {
        GroundCheck.onGrounded += ResetJump;
        PlayerController.onJump += Jump;
        onChangeJump += ChangeJump;
    }

    void OnDisable()
    {
        GroundCheck.onGrounded -= ResetJump;
        PlayerController.onJump -= Jump;
        onChangeJump -= ChangeJump;
    }

    public void Jump(bool isJumpPressed) // if you can jump : decremente from the count and do the jump
    {
        if (isJumpPressed)
        {
            if (nbJump <= 0) return;
            nbJump--;
            //Debug.Log($"nbJump = {nbJump}");

            Vector2 jumpDirection = new Vector2(0, jumpForce);
            rb.linearVelocityY = 0;
            isGrounded = false;
            rb.AddForce(jumpDirection, ForceMode2D.Impulse);

            JumpFX.onJump?.Invoke();
        }
        else
        {
            StopJump();
        }
    }

    private void StopJump() // if you stop your jump mid air you slow down his momentum
    {
        if (rb.linearVelocityY > 0) rb.linearVelocityY /= 2;
    }

}
